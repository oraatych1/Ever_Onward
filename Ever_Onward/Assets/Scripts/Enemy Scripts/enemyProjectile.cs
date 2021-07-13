using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectile : MonoBehaviour
{
    public float speed = 10f;   // Speed of the projectile
    public float lifetime = 5f; // How long the projectile will last.

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);  // Destroys the projectile after 5 seconds
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);  // Moves the projectile forward
    }
}
