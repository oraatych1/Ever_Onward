using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    bool canBeHurt = true;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && canBeHurt)
        {
            print("dd");
            HealthAndManaSystem.health--;
            canBeHurt = false;
            Invoke("CanHurtAgain", 1f);
        }
        //print("rtt");
    }
    void CanHurtAgain()
    {
        canBeHurt = true;
    }
}
