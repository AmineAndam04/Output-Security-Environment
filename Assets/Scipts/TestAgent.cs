using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TestAgent : Agent
{
    
    private  string[] tagsToFind = {"collaborative","Obscure","Distraction","Avatar","AvatarMalicious"} ;

    
     [SerializeField] private int maxState = 148;
    //[SerializeField] private int maxAction = 30;
    private Dictionary<string, int>  objectsCount = new Dictionary<string, int> ();
    //private Dictionary<string,List<List<float>>> stateSpace = new Dictionary<string,List<List<float>>>() ; 
    private float padValue = 999999f; 
    private int currentStateSize = 0;
    private int currentActionsSize = 0;
    
    private List<CollabObject> collabList = new List<CollabObject>();
    private List<ObscObject> obscList = new List<ObscObject>();
    private List<DistrcObject> distrcList = new List<DistrcObject>();
    private List<Avatar> avatarList = new List<Avatar>();
    private List<MaliciuosAvatar> malavatarList = new List<MaliciuosAvatar>();
    
    
    public override void CollectObservations(VectorSensor sensor)
    {
        currentStateSize = 0;
        currentActionsSize=0;
        collabList.Clear();
        obscList.Clear();
        distrcList.Clear();
        avatarList.Clear();
        malavatarList.Clear();
        foreach (string tag in tagsToFind)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            //Debug.Log(tag +"is" + objectsWithTag.Length);
            objectsCount[tag] =  objectsWithTag.Length;
            //Debug.Log("Nbr of "+ tag +" :"+objectsWithTag.Length);
            //List<List<float>> values = new List<List<float>>(); 
            
            if (tag == "collaborative")
            {
                
                foreach (GameObject gameObject in objectsWithTag)
                {
                    CollabObject collab = new CollabObject(gameObject);
                    collabList.Add(collab);
                    //sensor.AddObservation(gameObject.transform.localPosition);
                    sensor.AddObservation(collab.GetPosition());
                    //Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
                    //sensor.AddObservation(box["bounds"]);
                    sensor.AddObservation(collab.GetBB());
                    currentStateSize += 6;
                    
                    
                }
                
            }
            if (tag == "Obscure")
            {
                foreach (GameObject gameObject in objectsWithTag)
                {
                    ObscObject obsc = new ObscObject(gameObject);
                    obscList.Add(obsc);
                    //sensor.AddObservation(gameObject.transform.localPosition);
                    sensor.AddObservation(obsc.GetPosition());
                    //Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
                    //sensor.AddObservation(box["bounds"]);
                    sensor.AddObservation(obsc.GetBB());
                    //sensor.AddObservation(utils.GetAlpha(gameObject));
                    sensor.AddObservation(obsc.GetAlpha());
                    currentStateSize += 7;
                    currentActionsSize+=7;

                    
                }
            }
            if (tag == "Distraction" )
            {
                foreach (GameObject gameObject in objectsWithTag)
                {
                    DistrcObject distrc = new DistrcObject(gameObject);
                    distrcList.Add(distrc);
                    //sensor.AddObservation(gameObject.transform.localPosition);
                    //sensor.AddObservation(distrc.GetPosition());
                    //Dictionary<string, Vector3> box = utils.GetBoundingBox(gameObject);
                    //sensor.AddObservation(box["bounds"]);
                    sensor.AddObservation(distrc.GetBB());
                    //sensor.AddObservation(utils.GetFrequencies(gameObject));
                    sensor.AddObservation(distrc.Getfreq());
                    //sensor.AddObservation(utils.GetAlpha(gameObject));
                    //sensor.AddObservation(distrc.GetAlpha());
                    //currentStateSize += 9;
                    //currentActionsSize += 6;
                    currentStateSize += 5;
                    currentActionsSize += 5;
                    
                }
            }

            if (tag == "Avatar")
            {
               foreach (GameObject  gameObject in objectsWithTag)
               {
                  Avatar avatar = new Avatar(gameObject);
                  avatarList.Add(avatar);
                  //sensor.AddObservation(gameObject.transform.localPosition);
                  sensor.AddObservation(avatar.GetPosition());
                  currentStateSize += 3;
               }
               
                
            }
            if (tag == "AvatarMalicious")
            {
                foreach (GameObject  gameObject in objectsWithTag)
               {
                  MaliciuosAvatar malavatar = new MaliciuosAvatar(gameObject);
                  malavatarList.Add(malavatar);
                  //sensor.AddObservation(gameObject.transform.localPosition);
                  sensor.AddObservation(malavatar.GetPosition());
                  currentStateSize += 3;
                  currentActionsSize+=3;
               }
            }
            
                    
        }
        Debug.Log(currentStateSize);
        while(currentStateSize< maxState)
        {
            sensor.AddObservation(padValue);
            currentStateSize+=1;
        }
        Debug.Log(currentStateSize);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {   
        

        Vector3 positionShift ; //= new Vector3(0.1f,0.1f,0.1f);
        Vector3 bbShift ;
        float alphaShift;
        Vector2 freqshift ;

        //obscList[0].ShiftPosition(positionShift);
        // Act on Obscure view
        int actionsIndex = 0;
        foreach (ObscObject obsc in obscList)
        {
            positionShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            obsc.ShiftPosition(positionShift); 
            ++actionsIndex;
            bbShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            obsc.ShiftBB(bbShift); 
            ++actionsIndex;
            alphaShift = actions.ContinuousActions[actionsIndex];
            obsc.ShiftAlpha(alphaShift);
            ++actionsIndex;
        }
        
        foreach (DistrcObject distrc in distrcList)
        {
            bbShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            distrc.ShiftBB(bbShift); 
            ++actionsIndex;
            freqshift = new Vector2(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex]);
            distrc.ShiftFreq(freqshift);
            ++actionsIndex;
        }

        foreach (MaliciuosAvatar malavatar in malavatarList)
        {
            positionShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            malavatar.ShiftPosition(positionShift); 
            ++actionsIndex;
        }
        //Debug.Log("The final index is: " + actionsIndex);
        //Debug.Log("The number of actions: "+ currentActionsSize);

        

    }
}
