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
    private int countColor = 0;
    private int countFlicker = 0;

    public float generatPeriod = 5f;
    private List<GameObject> distractionObjects = new List<GameObject>();

    
    /*private void Start()
    {
        //UnityEngine.Random.InitState(randomSeed);
        StartCoroutine(InstantiatePrefabRoutine());
    }*/

    IEnumerator InstantiatePrefabRoutine()
    {
        while (countObjects < maxDistractionObject)
        {
            
            Vector3 userPosition = userToBeDistractedPosition.position;
            Vector3 objectPosition = new Vector3(Random.Range(-6f, 9f), Random.Range(3f, 9f), Random.Range(19f, 25f));;  // userToBeDistractedPosition.forward * 2.0f;
            int choice = Random.Range(0, 2);
            if (countColor == 3)
            {
                choice = 0;
            }
            if (countFlicker == 3)
            {
                choice = 1;
            }
            GameObject distractionObject;
            if (choice == 1)
            {
                distractionObject = Instantiate(prefabOfColor, objectPosition, Quaternion.identity);
                countObjects++;
                countColor++ ;
                //Debug.Log("Color: " + countColor);
            }
            else{
                distractionObject = Instantiate(prefabOfFlicker, objectPosition, Quaternion.identity);
                countObjects++;
                countFlicker++;
                //Debug.Log("flicker: " + countFlicker);
            }
            distractionObjects.Add(distractionObject);
            // Instantiate the prefab at the position of the empty GameObject
            // Instantiate(prefabToInstantiate, objectPosition, Quaternion.identity);
            // countObjects++;
            //Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(generatPeriod);
        }
    }
    public void ResetAttack()
    {
        StopCoroutine(InstantiatePrefabRoutine());
        if (distractionObjects.Count >0)
        {
            foreach (var distractionObject in distractionObjects)
            {
                    Destroy(distractionObject);
            }
            distractionObjects.Clear();
            countObjects = 0;
        }
        StartCoroutine(InstantiatePrefabRoutine());
        
    }
}
