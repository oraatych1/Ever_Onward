using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Vector3 velocity;

    private float lifeSpan = 3;
    private float age = 0;
    private bool isGrass = false;

    private void Start()
    {
        
    }

    public void InitBullet(Vector3 vel)
    {
        velocity = vel;
    }

    private void Update()
    {
        age += Time.deltaTime;

        if(age > lifeSpan)
        {
            Destroy(gameObject);
        }

        transform.position += velocity * 3 * Time.deltaTime;

        //if()
    }
}
