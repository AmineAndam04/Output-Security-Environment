using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class TestAgent : Agent
{
    
    private  string[] tagsToFind = {"collaborative","Obscure","Distraction","Avatar","AvatarMalicious"} ;
    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (string tag in tagsToFind)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            if (tag == "collaborative")
            {
                foreach (GameObject gameObject in objectsWithTag)
                {
                    sensor.AddObservation(gameObject.transform.localPosition);
                    Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
                    sensor.AddObservation(box["bounds"]);
                }
                
            }
            if (tag == "Obscure")
            {
                foreach (GameObject gameObject in objectsWithTag)
                {
                    sensor.AddObservation(gameObject.transform.localPosition);
                    Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
                    sensor.AddObservation(box["bounds"]);
                    sensor.AddObservation(utils.GetAlpha(gameObject));
                }
            }
            if (tag == "Distraction" )
            {
                foreach (GameObject gameObject in objectsWithTag)
                {
                    sensor.AddObservation(gameObject.transform.localPosition);
                    Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
                    sensor.AddObservation(box["bounds"]);
                    sensor.AddObservation(utils.GetFrequencies(gameObject));
                }
            }
            
            Debug.Log("the number of " + tag + "is = " + objectsWithTag.Length);         
        }
    }
}
