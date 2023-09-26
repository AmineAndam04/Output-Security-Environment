using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaliciousObjectGenerator : MonoBehaviour
{
    public GameObject collaborativeObject; // Reference to the collaborative object.
    public GameObject maliciousObjectPrefab; // Prefab of the malicious object.
    public float orbitSpeed = 20f; // Speed of the orbit.
    public float orbitRadius = 5f; // Radius of the orbit.
    public float translationSpeed = 2.0f; // Speed of translation.
    private GameObject maliciousObject;
    private Vector3 targetPosition;
    private Vector3 initialPosition ;

    private float journeyLength;
    private float startTime;
    private bool isMoving = false;
    
    private void Start()
    {
        // Start generating malicious objects.
        
        StartCoroutine(GenerateMaliciousObjects());
    }

    /// <summary>
    /// Generate a malicious object. A new object is generated every 5 seconds
    /// </summary>
    private IEnumerator GenerateMaliciousObjects()
    {
        while (true)
        {
            // Generate malicious object within the bounding box of the collaborative object.
            initialPosition = GenerateRandomPositionInsideBoundingBox(collaborativeObject);

            // Instantiate the malicious object.
             maliciousObject = Instantiate(maliciousObjectPrefab, initialPosition, Quaternion.identity);
            // Set the target position for translation.
            targetPosition = GenerateRandomPositionInsideBoundingBox(collaborativeObject);

            // Calculate the distance the malicious object needs to travel.
            journeyLength = Vector3.Distance(initialPosition, targetPosition);

            // Start moving the malicious object.
            isMoving = true;
            startTime = Time.time;

            // Make the malicious object orbit around the collaborative object.
            //StartCoroutine(OrbitMaliciousObject(maliciousObject));

            // Wait for some time before generating the next malicious object.
            yield return new WaitForSeconds(5.0f);
        }
    }

    /// <summary>
    /// Generate a random position within the bounding boxes of a collaborative object
    /// </summary>
    /// <param name="gameObject">The collaborative object</param>
    /// <returns> The initial position of the malicious object</returns>
    
    private Vector3 GenerateRandomPositionInsideBoundingBox(GameObject gameObject)
    {
        Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
        Vector3 Boundmin = box["min"];
        Vector3 Boundmax = box["max"];
        //Debug.Log(Boundmax);

        //if (box.Count == 0)
        //{
            

            // Generate random position within the bounds of the collaborative object's bounding box.
            float randomX = Random.Range(Boundmin.x, Boundmax.x);
            float randomY = Random.Range(Boundmin.y, Boundmax.y);
            float randomZ = Boundmin.z - 0.5f;

            return new Vector3(randomX, randomY, randomZ);
        //}
        //else
        //{
            //Debug.LogError("The collaborative object does not have a BoxCollider component.");
            //return Vector3.zero;
        //}
    }

    private void Update()
    {
        if (isMoving)
        {
            // Calculate the fraction of the journey covered so far.
            float distanceCovered = (Time.time - startTime) * translationSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            // Interpolate the malicious object's position.
            maliciousObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, fractionOfJourney);

            // Check if the malicious object has reached the destination.
            if (fractionOfJourney >= 1.0f)
            {
                isMoving = false; // Stop moving.
            }
        }
    }
    private IEnumerator OrbitMaliciousObject(GameObject maliciousObject)
    {
        while (true)
        {
            Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
            Vector3 Boundmin = box["max"];
            Vector3 randomPosition = GenerateRandomPositionInsideBoundingBox(collaborativeObject);
            randomPosition.z = Boundmin.z - 0.5f;

            // Calculate the vector from the malicious object to the center point.
            Vector3 toCenter = randomPosition - maliciousObject.transform.position;

            // Project the vector onto the XY plane.
            //toCenter.y = 0f;

            // Calculate the normalized direction for orbiting on the XY plane.
            Vector3 orbitDirection = toCenter.normalized;

            // Move the malicious object continuously along the XY plane.
            maliciousObject.transform.Translate(orbitDirection * orbitSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
    }
}
