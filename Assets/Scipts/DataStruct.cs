using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct CollabObject
{
    public GameObject gameObject;

    public CollabObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.localPosition;
        
    }
    public Vector3 GetBB()
    {
        Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
        return box["bounds"];
    }

}

public struct ObscObject
{
    public GameObject gameObject;

    public ObscObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.localPosition;
        
    }
    public Vector3 GetBB()
    {
        Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
        return box["bounds"];
    }

    public float GetAlpha()
    {
        float alpha = utils.GetAlpha(gameObject);
        return alpha;
    }
}


public struct DistrcObject
{
    public GameObject gameObject;

    public DistrcObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.localPosition;
        
    }
    public Vector3 GetBB()
    {
        Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
        return box["bounds"];
    }

    public float GetAlpha()
    {
        float alpha = utils.GetAlpha(gameObject);
        return alpha;
    }
    public Vector2 Getfreq()
    {
        Vector2 freq = utils.GetFrequencies(gameObject);
        return freq;
    }
}

public struct Avatar
{
    public GameObject gameObject;

    public Avatar(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    public Vector3 GetPosition()
    {
        return gameObject.transform.localPosition;
        
    }
}

public struct MaliciuosAvatar
{
    public GameObject gameObject;

    public MaliciuosAvatar(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    public Vector3 GetPosition()
    {
        return gameObject.transform.localPosition;
        
    }
}
