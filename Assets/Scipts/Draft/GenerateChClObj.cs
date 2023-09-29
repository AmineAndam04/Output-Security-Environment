using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    public Transform userToBeDistractedPosition ;
    public int maxDistractionObject = 5 ;
    public int countObjects =0 ;

    public float generatPeriod = 5f;
    
    private void Start()
    {
        StartCoroutine(InstantiatePrefabRoutine());
    }

    IEnumerator InstantiatePrefabRoutine()
    {
        while (countObjects < maxDistractionObject)
        {
            
            Vector3 userPosition = userToBeDistractedPosition.position;
            Vector3 objectPosition = userPosition + new Vector3(Random.Range(-5f, 5f), Random.Range(1f, 5f), Random.Range(0f, 5f));;  // userToBeDistractedPosition.forward * 2.0f;
            
            // Instantiate the prefab at the position of the empty GameObject
            Instantiate(prefabToInstantiate, objectPosition, Quaternion.identity);
            countObjects++;
            //Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(generatPeriod);
        }
    }
}
