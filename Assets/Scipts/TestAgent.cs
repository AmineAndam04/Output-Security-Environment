using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class TestAgent : Agent
{
    
    private  string[] tagsToFind = {"collaborative","Obscure","Distraction","Avatar","AvatarMalicious"} ;
     [SerializeField] private int maxState = 172;
    [SerializeField] private int maxAction = 30;
    private Dictionary<string, int>  objectsCount = new Dictionary<string, int> ();
    private float padValue = 999999f; 
    private int currentStateSize = 0;
    public override void CollectObservations(VectorSensor sensor)
    {
        currentStateSize = 0;
        foreach (string tag in tagsToFind)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            //Debug.Log(tag +"is" + objectsWithTag.Length);
            objectsCount[tag] =  objectsWithTag.Length;
            //Debug.Log("Nbr of "+ tag +" :"+objectsWithTag.Length);
            if (tag == "collaborative")
            {
                
                foreach (GameObject gameObject in objectsWithTag)
                {
                    sensor.AddObservation(gameObject.transform.localPosition);
                    Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
                    sensor.AddObservation(box["bounds"]);
                    currentStateSize += 6;
                    
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
                    currentStateSize += 7;
                    
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
                    sensor.AddObservation(utils.GetAlpha(gameObject));
                    currentStateSize += 9;
                    
                }
            }

            if (tag == "Avatar" || tag ==  "AvatarMalicious")
            {
               foreach (GameObject  gameObject in objectsWithTag)
               {
                  sensor.AddObservation(gameObject.transform.localPosition);
                  currentStateSize += 3;
               }
                
            }
            
                    
        }
        
        while(currentStateSize< maxState)
        {
            sensor.AddObservation(padValue);
            currentStateSize+=1;
        }
        Debug.Log(currentStateSize);
    }
}
