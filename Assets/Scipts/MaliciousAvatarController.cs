using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaliciousAvatarController : MonoBehaviour
{
   public GameObject maliciousAvatarPrefab;
    public float delayInSeconds = 5f;

    private void Start()
    {
        // Start a coroutine to spawn the malicious avatar after the delay
        StartCoroutine(SpawnMaliciousAvatar());
    }

    private IEnumerator SpawnMaliciousAvatar()
    {
        yield return new WaitForSeconds(delayInSeconds);

        // Instantiate the malicious avatar and set its position
        GameObject maliciousAvatar = Instantiate(maliciousAvatarPrefab, transform.position, Quaternion.identity);

        // Ensure the malicious avatar is active
        maliciousAvatar.SetActive(true);
    }
}
