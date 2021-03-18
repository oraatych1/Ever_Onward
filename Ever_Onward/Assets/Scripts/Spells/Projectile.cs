using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

       PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player)
        {
          HealthAndManaSystem playerHealth =  player.GetComponent<HealthAndManaSystem>();
            if (playerHealth)
            {
                playerHealth.TakeDamage(10);
                
            }
            Destroy(gameObject);
        }

    }
}
