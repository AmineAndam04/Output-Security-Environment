using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject prefabOfColor;
    public GameObject prefabOfFlicker ;
    public Transform userToBeDistractedPosition ;
    public int maxDistractionObject = 5 ;
    private int countObjects =0 ;
    //public int randomSeed = 12678;

    public float generatPeriod = 5f;
    
    private void Start()
    {
        //UnityEngine.Random.InitState(randomSeed);
        StartCoroutine(InstantiatePrefabRoutine());
    }

    IEnumerator InstantiatePrefabRoutine()
    {
        while (countObjects < maxDistractionObject)
        {
            
            Vector3 userPosition = userToBeDistractedPosition.position;
            Vector3 objectPosition = new Vector3(Random.Range(-6f, 9f), Random.Range(3f, 9f), Random.Range(18f, 25f));;  // userToBeDistractedPosition.forward * 2.0f;
            int choice = Random.Range(0, 2);
            if (choice == 1)
            {
                Instantiate(prefabOfColor, objectPosition, Quaternion.identity);
                countObjects++;
            }
            else{
                Instantiate(prefabOfFlicker, objectPosition, Quaternion.identity);
                countObjects++;
            }
            // Instantiate the prefab at the position of the empty GameObject
            // Instantiate(prefabToInstantiate, objectPosition, Quaternion.identity);
            // countObjects++;
            //Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(generatPeriod);
        }
    }
}
