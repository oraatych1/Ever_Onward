using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantDetector : MonoBehaviour
{
    // Start is called before the first frame update

    Animator anim;
    Renderer rend;
    Collider m_Collider;

    void Start()
    {
        anim = GetComponent<Animator>();
        //rend = GetComponent<Renderer>();
        m_Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Grass")
        {
            anim.SetBool("isWilted", false);
            //rend.material.color = Color.red;
            Invoke("SetAddCollision", 2f);
            Invoke("DisableCollision", 15f);
        }
    }

    void SetAddCollision()
    {
        m_Collider.enabled = true;
    }

    void DisableCollision()
    {
        anim.SetBool("isWilted", true);
        //rend.material.color = Color.gray;
        m_Collider.enabled = false;
    }
}
