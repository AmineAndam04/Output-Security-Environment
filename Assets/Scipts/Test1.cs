using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class UserHarassmentAttack3 : MonoBehaviour
{
    public float spawnDelay = 5f;
    public float attackPeriod = 2f;
    public int numAttacks = 3;
    public float privateSpace = 2f;

    private GameObject maliciousUser;
    private List<GameObject> avatars;

    void Start()
    {
        Invoke("SpawnMaliciousUser3", spawnDelay);

        avatars = new List<GameObject>();
        foreach (GameObject avatar in GameObject.FindGameObjectsWithTag("Avatar"))
        {
            avatars.Add(avatar);
        }
    }

    void SpawnMaliciousUser3()
    {
        // Spawn the malicious user at a random position in the scene.
        maliciousUser = Instantiate(new GameObject());
        maliciousUser.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));

        // Add a tag to the malicious user so that we can easily identify it later.
        maliciousUser.tag = "MaliciousUser";
    }

    void Update()
    {
        // If the malicious user exists, move it towards a random avatar and attack it.
        if (maliciousUser != null)
        {
            // Choose a random avatar.
            GameObject targetAvatar = avatars[Random.Range(0, avatars.Count)];

            // Get the distance between the malicious user and the target avatar.
            float distance = Vector3.Distance(maliciousUser.transform.position, targetAvatar.transform.position);

            // If the distance is less than the malicious user's personal space, attack the target avatar.
            if (distance < privateSpace)
            {
                // Attack the target avatar for a period of time.
                Invoke("StopAttacking", attackPeriod);

                // Start moving towards the next avatar.
                StartCoroutine(MoveToNextAvatar());
            }
            else
            {
                // Move towards the target avatar.
                maliciousUser.transform.position = Vector3.Lerp(maliciousUser.transform.position, targetAvatar.transform.position, Time.deltaTime);
            }
        }
    }

    void StopAttacking()
    {
        // Stop attacking the target avatar.
        maliciousUser.GetComponent<Animator>().SetBool("IsAttacking", false);
    }

    IEnumerator MoveToNextAvatar()
    {
        // Wait for a short period of time before moving towards the next avatar.
        yield return new WaitForSeconds(0.5f);

        // Choose a new random avatar.
        GameObject targetAvatar = avatars[Random.Range(0, avatars.Count)];

        // Start moving towards the new target avatar.
        StartCoroutine(MoveToAvatar(targetAvatar));
    }

    IEnumerator MoveToAvatar(GameObject targetAvatar)
    {
        // Move towards the target avatar until we reach its personal space.
        while (Vector3.Distance(maliciousUser.transform.position, targetAvatar.transform.position) >= privateSpace)
        {
            maliciousUser.transform.position = Vector3.Lerp(maliciousUser.transform.position, targetAvatar.transform.position, Time.deltaTime);

            yield return null;
        }

        // Attack the target avatar.
        maliciousUser.GetComponent<Animator>().SetBool("IsAttacking", true);
    }
}
