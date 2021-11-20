using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantDetector : MonoBehaviour
{
    // Start is called before the first frame update


    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>(); 
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Grass")
        {
            rend.material.color = Color.red;
        }
    }
}
