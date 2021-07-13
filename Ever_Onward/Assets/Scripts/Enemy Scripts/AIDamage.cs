using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        thisMat = GetComponent<Renderer>().material;
        currentlyAssignedMaterials = GetComponent<Renderer>().materials;
        theseMats = GetComponent<Renderer>().materials;
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
            print(currentlyAssignedMaterials);
            if (health <= 0)
            {
                for (int i = 50; i > 0; i--)
                {
                    Instantiate(cube, this.transform.position, this.transform.rotation);
                }
                Destroy(gameObject);
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wind")
        {
            GetComponent<Renderer>().materials = theseMats;
            rend.materials = theseMats;
        }
    }
}
