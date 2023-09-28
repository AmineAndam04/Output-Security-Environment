using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public Renderer render;
    //private Renderer render;

    void Start()
    {
        //Renderer render = GetComponent<Renderer>();
        StartCoroutine(ChangeColorRoutine());
    }

    IEnumerator ChangeColorRoutine()
    {
        while (true)
        {
            //Color color = render.material.color;
            //color.r = Random.value; // Change the red component to a random value
            Color color = new Color(Random.value, Random.value, Random.value);
            render.material.color = color; // Apply the new color to the material

            yield return new WaitForSeconds(0.1f);
        }
    }
}

