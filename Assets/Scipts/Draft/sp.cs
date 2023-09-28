using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    
    private void Start()
    {
        StartCoroutine(InstantiatePrefabRoutine());
    }

    IEnumerator InstantiatePrefabRoutine()
    {
        while (true)
        {
            // Instantiate the prefab at the position of the empty GameObject
            Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(5f);
        }
    }
}
