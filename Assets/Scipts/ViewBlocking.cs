using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBlocking : MonoBehaviour
{
    public GameObject maliciousObjectPrefab ; // The prefab of the malicious object
    public GameObject collaboObjects;
    private BoxCollider objectCollider;

    void GetCoordinates(GameObject Obj){
        

    }

     //void Start()
     //{
        //Vector3 objectPosition = collaboObjects.transform.position;
        //Debug.Log("Malicious Object's Position: " + objectPosition);

        //BoxCollider boxCollider = collaboObjects.GetComponent<BoxCollider>();
        //if (boxCollider != null)
        //{
          //  Bounds objectBounds = new Bounds(boxCollider.center, boxCollider.size);
           // Debug.Log("Object's Bounding Box: " + objectBounds);
        //}
        //else
        //{
          //  Debug.LogWarning("Object does not have a Box Collider.");
        //}
    //}
    private void Start()
    {
        // Get the BoxCollider component of the object
        objectCollider = collaboObjects.GetComponent<BoxCollider>();

        if (objectCollider == null)
        {
            Debug.LogError("This script requires a BoxCollider component on the GameObject.");
            return;
        }

        // Calculate the bounding box in world space
        Bounds bounds = objectCollider.bounds;

        // Extract the min and max points of the bounding box
        Vector3 minPoint = bounds.min;
        Vector3 maxPoint = bounds.max;

        // Now you have the x, y, and z min and max values
        float xMin = minPoint.x;
        float yMin = minPoint.y;
        float zMin = minPoint.z;

        float xMax = maxPoint.x;
        float yMax = maxPoint.y;
        float zMax = maxPoint.z;

        // Print or use these values as needed
        Debug.Log("x_min: " + xMin);
        Debug.Log("y_min: " + yMin);
        Debug.Log("z_min: " + zMin);
        Debug.Log("x_max: " + xMax);
        Debug.Log("y_max: " + yMax);
        Debug.Log("z_max: " + zMax);
    }
}

