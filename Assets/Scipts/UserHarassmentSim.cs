using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;

public class UserHarassmentSim : MonoBehaviour {

    public GameObject malicousAvatarPrefab ;
    private GameObject maliciousAvatar;
    private float spawnDelay = 5f;
    private Vector3 initialPosition = new Vector3(0f,0f,0f);

    private GameObject[] otherAvatars;
    public float  privateSpace = 1f;
    private GameObject targetedAvatar;
    public float attackPeriod = 10f ;

    

    void   Start() {
        Invoke("SpawnMaliciousAvatar", spawnDelay);
        otherAvatars = GameObject.FindGameObjectsWithTag("Avatar");
        for (int i = 0; i < otherAvatars.Length; i++)
        {
            Debug.Log(otherAvatars[i].transform.position);
        }
    }
    void SpawnMaliciousAvatar(){
        // Transform initialTransform ;
        //initialTransform.position = initialPosition;

        // Instantiate the malicious avatar at the predefined position
        maliciousAvatar = Instantiate(malicousAvatarPrefab, initialPosition, Quaternion.identity);

        
    }


}