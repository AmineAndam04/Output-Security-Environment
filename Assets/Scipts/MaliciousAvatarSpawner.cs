using UnityEngine;
using System.Collections;

public class MaliciousAvatarSpawner : MonoBehaviour
{
    public GameObject maliciousAvatarPrefab;
    public Vector3 initialPosition = new Vector3(0f, 0f, 0f); // Initialize to (0, 0, 0)
    public float delayInSeconds = 5f;

    private GameObject[] otherAvatars; // Array to store other avatars

    private void Awake()
    {
        // Find all other avatars in the scene and store them in the array
        otherAvatars = GameObject.FindGameObjectsWithTag("Avatar");
    }

    private void Start()
    {
        // Start a coroutine to spawn the malicious avatar after the delay
        StartCoroutine(SpawnMaliciousAvatar());
    }

    private IEnumerator SpawnMaliciousAvatar()
    {
        yield return new WaitForSeconds(delayInSeconds);

        // Create a new Transform with the specified initial position
        Transform initialTransform = new GameObject().transform;
        initialTransform.position = initialPosition;

        // Instantiate the malicious avatar at the predefined position
        GameObject maliciousAvatar = Instantiate(maliciousAvatarPrefab, initialTransform.position, initialTransform.rotation);

        // Ensure the malicious avatar is active
        maliciousAvatar.SetActive(true);

        // Attach the behavior script to the malicious avatar
        MaliciousAvatarBehavior behaviorScript = maliciousAvatar.AddComponent<MaliciousAvatarBehavior>();

        // Pass the other avatars to the behavior script
        behaviorScript.otherAvatars = otherAvatars;

        // Destroy the temporary Transform created for the initial position
        Destroy(initialTransform.gameObject);
    }
}

