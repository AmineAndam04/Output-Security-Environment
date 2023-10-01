using UnityEngine;
using System.Collections;

public class MaliciousAvatarBehavior : MonoBehaviour
{
    private Transform currentTarget; // The current target avatar
    private bool isAttacking = false; // Flag to track whether the avatar is attacking
    public float moveSpeed = 5.0f; // Adjust the move speed as needed
    public float attackPeriod = 2.0f; // Time to stay next to the target before moving to another
    public GameObject[] otherAvatars; // Array to store other avatars

    private void Awake()
    {
        // Find all other avatars in the scene and store them in the array
        otherAvatars = GameObject.FindGameObjectsWithTag("Avatar");
    }

    private void Start()
    {
        // Start the behavior
        StartCoroutine(AttackRandomTarget());
    }

    private IEnumerator AttackRandomTarget()
    {
        while (true)
        {
            // Check if there are any other avatars in the array
            if (otherAvatars.Length > 0)
            {
                // Choose a random target from the other avatars
                currentTarget = otherAvatars[Random.Range(0, otherAvatars.Length)].transform;

                // Print the name of the targeted avatar for debugging
                Debug.Log("Targeting: " + currentTarget.name);

                // Reset the isAttacking flag
                isAttacking = false;

                // Continuously move towards the current target
                while (!isAttacking)
                {
                    MoveTowardsTarget();

                    // Check the distance to the current target
                    float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

                    // If the avatar is close to the target, break out of the loop
                    if (distanceToTarget < 0.5f)
                    {
                        break;
                    }

                    yield return null;
                }
            }
            else
            {
                // No other avatars found, wait for a while and continue searching
                yield return new WaitForSeconds(Random.Range(3f, 5f));
            }
        }
    }

    private void MoveTowardsTarget()
    {
        if (currentTarget != null)
        {
            // Calculate the direction to the target
            Vector3 targetPosition = currentTarget.position;

            // Calculate the new position to move towards the target using Lerp
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Move the avatar to the new position
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Avatar") && !isAttacking)
        {
            // If the malicious avatar collides with the target and is not currently attacking
            // Stop moving and start the attack period
            isAttacking = true;
            StartCoroutine(AttackPeriod());
        }
    }

    private IEnumerator AttackPeriod()
    {
        // Stay next to the target for the specified attack period
        yield return new WaitForSeconds(attackPeriod);

        // Reset the currentTarget to null, allowing the avatar to choose a new target
        currentTarget = null;

        // Resume moving after the attack period is over
        isAttacking = false;
    }
}

