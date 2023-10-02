using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserHarassmentSim : MonoBehaviour
{
    public GameObject maliciousAvatarPrefab;
    private GameObject maliciousAvatar;
    private float spawnDelay = 5f;
    private Vector3 initialPosition = new Vector3(0f, 0f, 0f);

    private GameObject[] otherAvatars;
    public float privateSpace = 0.5f;
    private GameObject targetedAvatar;
    public float attackPeriod = 50f;
    private GameObject previousTargetedAvatar;

    void Start()
    {
        Invoke("SpawnMaliciousAvatar", spawnDelay);
        otherAvatars = GameObject.FindGameObjectsWithTag("Avatar");
    }

    void SpawnMaliciousAvatar()
    {
        maliciousAvatar = Instantiate(maliciousAvatarPrefab, initialPosition, Quaternion.identity);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (maliciousAvatar != null)
        {
            // Randomly choose a new targeted avatar only when the previous attack is over.
            if (targetedAvatar == null)
            {
                do
                {
                    targetedAvatar = otherAvatars[Random.Range(0, otherAvatars.Length)];
                } while (targetedAvatar == previousTargetedAvatar); // Ensure the new target is not the same as the previous one

                Debug.Log("New Target: " + targetedAvatar.transform.position);
                //Debug.Log("Distance btw them is:" + Vector3.Distance(maliciousAvatar.transform.position,targetedAvatar.transform.position));
                previousTargetedAvatar = targetedAvatar;
            }

            float distance = Vector3.Distance(maliciousAvatar.transform.position, targetedAvatar.transform.position);
            

            if (distance <    privateSpace)
            {
                
                // Attack the target avatar.
                Transform privateSpaceSphere = targetedAvatar.transform.Find("PrivateSpace"); 
                Renderer sphereRenderer = privateSpaceSphere.GetComponent<Renderer>();
                Color originalColor = sphereRenderer.material.color; 
                sphereRenderer.material.color = new Color(1f, 0f, 0f, sphereRenderer.material.color.a);
;
                yield return new WaitForSeconds(attackPeriod);

                // Reset the targeted avatar to null to choose a new target in the next attack.
                targetedAvatar = null;
                sphereRenderer.material.color = originalColor ; 
            }
            else
            {
                maliciousAvatar.transform.position = Vector3.Lerp(maliciousAvatar.transform.position, targetedAvatar.transform.position, Time.deltaTime);
                Debug.Log("Distance" + distance);
            }

            yield return null;
        }
    }
}
