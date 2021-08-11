using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDamage : MonoBehaviour
{
    public GameObject cube;
    private int health = 3;
    private Material thisMat;
    public Material thatMat;
    Material[] currentlyAssignedMaterials;
    public Material[] thoseMats;
    Material[] theseMats;
    Renderer rend;

    private float siphonStunTimer = 3f;
    public bool isStunned = false;
    private float saveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        thisMat = GetComponent<Renderer>().material;
        currentlyAssignedMaterials = GetComponent<Renderer>().materials;
        theseMats = GetComponent<Renderer>().materials;
    }

    private void Update()
    {
        if(isStunned)
        {
            if (GetComponentInParent<NavMeshAgent>().speed != 0) saveSpeed = GetComponentInParent<NavMeshAgent>().speed;
            
            GetComponentInParent<NavMeshAgent>().speed = 0;
            siphonStunTimer -= Time.deltaTime;
            if(siphonStunTimer <= 0)
            {
                GetComponentInParent<NavMeshAgent>().speed = saveSpeed;
                isStunned = false;
                siphonStunTimer = 3f;

            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wind")
        {
            health--;

            //currentlyAssignedMaterials[0] = thatMat;
            //currentlyAssignedMaterials[1] = thatMat;
            GetComponent<Renderer>().materials = thoseMats;
            rend.materials = thoseMats; 
            if (health <= 0)
            {
                for (int i = 50; i > 0; i--)
                {
                    Instantiate(cube, this.transform.position, this.transform.rotation);
                }
                Destroy(gameObject);
            }

        }

        if(other.tag == "Bramble")
        {
            health -= 3;
            GetComponent<Renderer>().materials = thoseMats;
            rend.materials = thoseMats;
            if (health <= 0)
            {
                for (int i = 50; i > 0; i--)
                {
                    Instantiate(cube, this.transform.position, this.transform.rotation);
                }
                Destroy(gameObject);
            }
        }

        if(other.tag == "Siphon")
        {
            health--;
            GetComponent<Renderer>().materials = thoseMats;
            rend.materials = thoseMats;
            if (health <= 0)
            {
                for (int i = 50; i > 0; i--)
                {
                    Instantiate(cube, this.transform.position, this.transform.rotation);
                }
                Destroy(gameObject);
            }
            isStunned = true;
            siphonStunTimer = 3f;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wind" || other.tag == "Bramble" || other.tag == "Siphon")
        {
            GetComponent<Renderer>().materials = theseMats;
            rend.materials = theseMats;
        }
    }
}
