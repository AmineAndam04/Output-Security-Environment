using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using Unity.MLAgents.SideChannels;

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
    private float alphaThreshold = 0.1f; 
    private float cf0 = 1/20 ;
    private float bf0 = 1/20;
    private float du = 1.5f;
    private List<float> rewardWeights = new List<float> {1.0f,1.0f,1.0f,1.0f,1.0f,1.0f}; 
    private SendHyperParameters sendHyperParameters;

    private List<float> hyperParameters ;
    
    public override void Initialize()
    {
        sendHyperParameters = new SendHyperParameters();
        SideChannelManager.RegisterSideChannel(sendHyperParameters);
        hyperParameters = sendHyperParameters.GetReceivedHyperParameters();
        int numHyperParameters = (int) hyperParameters[0];
        alphaThreshold = hyperParameters[1];
        cf0 = hyperParameters[2];
        bf0 = hyperParameters[3];
        du = hyperParameters[4];
        int i = 0;
        while (i< rewardWeights.Count)
        {
            rewardWeights[i] = hyperParameters[i+5];
        }


    }
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
                    sensor.AddObservation(collab.GetBB()["bounds"]);
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
                    sensor.AddObservation(obsc.GetBB()["bounds"]);
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
        //Debug.Log(currentStateSize);
        while(currentStateSize< maxState)
        {
            sensor.AddObservation(padValue);
            currentStateSize+=1;
        }
        //Debug.Log(currentStateSize);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {   
        

        Vector3 positionShift ; 
        Vector3 bbShift ;
        float alphaShift;
        Vector2 freqshift ;
        // Compute Ixy and Iyz for View blocking elements
        Dictionary<CollabObject, List<float>>  Ixyz = new Dictionary<CollabObject, List<float>>() ;
        List<Vector3> initialPosition = new  List<Vector3>() ;
        List<float> rewads = new List<float>() ;
        
        foreach (CollabObject collab in collabList)
        {
            List<float> I = new List<float>() ;
            foreach (ObscObject obsc in obscList)
            {
                Vector2 ixyz= utils.Ixyz(collab.gameObject,obsc.gameObject);
                I.Add(ixyz.x);
                I.Add(ixyz.y);
            }
            Ixyz.Add(collab,I);
        }

        



        float Reward2 = 0f;
        // Act on Obscure view
        int actionsIndex = 0;
        foreach (ObscObject obsc in obscList)
        {
            positionShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            obsc.ShiftPosition(positionShift); 
            Reward2 -= Vector3.Distance(positionShift, new Vector3(0f,0f,0f));
            ++actionsIndex;
            bbShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            obsc.ShiftBB(bbShift); 
            ++actionsIndex;
            alphaShift = actions.ContinuousActions[actionsIndex];
            obsc.ShiftAlpha(alphaShift);
            ++actionsIndex;
        }
        

        // Compute the rewards for View Blocking attack
        float Reward1=0f ;
        foreach (CollabObject collab in collabList)
        {
            int i = 0;
            foreach (ObscObject obsc in obscList)
            {
                Vector3 w = collab.GetPosition();
                Vector3 o = obsc.GetPosition();
                Vector3 bb = obsc.GetBB()["bounds"];
                float volume = bb.x * bb.y * bb.z ;
                if (Ixyz[collab][i]==1)
                {
                    Vector2 axyz= utils.Axyz(collab.gameObject,obsc.gameObject);
                    float axy = axyz.x;
                    float dxy = Vector2.Distance(new Vector2(w.x,w.y),new Vector2(o.x,o.y));
                    
                    Reward1 += dxy - axy - volume;


                }
                ++i;
                if (Ixyz[collab][i]==1)
                {
                    Vector2 axyz= utils.Axyz(collab.gameObject,obsc.gameObject);
                    float ayz = axyz.y;
                    float dyz = Vector2.Distance(new Vector2(w.y,w.z),new Vector2(o.y,o.z));
                    Reward1 += dyz - ayz - volume;
                }
                ++i;
            }
            
        }
        AddReward(rewardWeights[0]*Reward1);
        rewads.Add(Reward1);
        AddReward(rewardWeights[1]*Reward2);
        rewads.Add(Reward2);
        float Reward3 = 0f ;
        
        foreach (CollabObject collab in collabList)
        {
            int i=0;
            foreach (ObscObject obsc in obscList)
            {
                Reward3 -= (Ixyz[collab][i] + Ixyz[collab][i+1]) *utils.ReLU(obsc.GetAlpha() - alphaThreshold);
                i+=2;
            }
        }
        AddReward(rewardWeights[2]*Reward3);
        rewads.Add(Reward3);

        // Act in distraction objects 
        foreach (DistrcObject distrc in distrcList)
        {
            bbShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            distrc.ShiftBB(bbShift); 
            ++actionsIndex;
            freqshift = new Vector2(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex]);
            distrc.ShiftFreq(freqshift);
            ++actionsIndex;
        }

        // Compute Reward4
        float Reward4 = 0f;
        float Reward5 = 0f;
        foreach (DistrcObject distrc in distrcList)
        {
            Vector2 freqs =  distrc.Getfreq();
            Reward5 -= utils.ReLU(freqs.x - cf0);
            Reward4 -= utils.ReLU(freqs.x - bf0);
            
        }
        AddReward(rewardWeights[3]*Reward4);
        rewads.Add(Reward4);
        AddReward(rewardWeights[4]*Reward5);
        rewads.Add(Reward5);

        foreach (MaliciuosAvatar malavatar in malavatarList)
        {
            positionShift = new Vector3(actions.ContinuousActions[actionsIndex],actions.ContinuousActions[++actionsIndex],actions.ContinuousActions[++actionsIndex]);
            malavatar.ShiftPosition(positionShift); 
            ++actionsIndex;
        }

        float Reward6 = 0f;
        foreach (Avatar avatar in avatarList)
        {
            if (malavatarList.Count != 0)
            {
                Reward6 -= utils.ReLU(du - Vector3.Distance(avatar.GetPosition(),malavatarList[0].GetPosition()));
            }
            
        }
        AddReward(rewardWeights[5]*Reward6);
        rewads.Add(Reward6);
        sendHyperParameters.SendIndividualRewards(rewads);
    }
}
