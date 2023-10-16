using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class utils
{
    public static Dictionary<string, Vector3> GetBoundingBox(GameObject gameObject)
    {

        BoxCollider objectCollider = gameObject.GetComponent<BoxCollider>(); ;
        Dictionary<string, Vector3> boundingBox = new Dictionary<string, Vector3>();



        if (objectCollider == null)
        {
            Debug.LogError("This script requires a BoxCollider component on the GameObject.");
            return boundingBox;
        }

        // Calculate the bounding box in world space
        Bounds bounds = objectCollider.bounds;

        // Extract the min and max points of the bounding box
        Vector3 minPoint = bounds.min;
        Vector3 maxPoint = bounds.max;

        boundingBox.Add("min", minPoint);
        boundingBox.Add("max", maxPoint);
        boundingBox.Add("bounds", maxPoint - minPoint);

        return boundingBox;
    }
    public static float GetAlpha(GameObject gameObject)
    {
        Renderer objectRenderer = gameObject.GetComponent<Renderer>();
        Color objectColor = objectRenderer.material.color;
        return objectColor.a;
    }
    public static Vector2 GetFrequencies(GameObject gameObject)
    {
        ColorChange colorChangeScript;
        FlickerChange flickerChangeScript;
        colorChangeScript = gameObject.GetComponent<ColorChange>();
        flickerChangeScript = gameObject.GetComponent<FlickerChange>();
        float colorFrequency = 0f ;
        float flickerFrequency = 0f ;
        if (colorChangeScript != null)
        {
             colorFrequency = colorChangeScript.colorfrequencyChange;
             flickerFrequency = 0f;

        }
        if (flickerChangeScript != null)
        {
            colorFrequency = 0f;
            flickerFrequency = flickerChangeScript.flickerfrequencyChange;
        }
        Vector2 ret ;
        ret.x = colorFrequency;
        ret.y = flickerFrequency;
        return ret;
    
    }


}
