using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{

    private float lifeSpan = 1f;
    private float age = 0;
    private int maxRadius = 2;
    private int enemiesInRange = 0;
    //private List<GameObject> hitEnemies = new List<GameObject>();

    public string type = "siphon";
    
    
    // Start is called before the first frame update
    void Start()
    {
        switch(type)
        {
            case "bramble":
                maxRadius = 2;
                break;
            case "siphon":
                maxRadius = 3;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        age += Time.deltaTime;

        if (age > lifeSpan)
        {
            print(enemiesInRange);
            if (type == "siphon") HealthAndManaSystem.health += enemiesInRange;
            Destroy(gameObject);
        }
        Vector3 newScale = new Vector3(Lerp(.5f, 3f, age * maxRadius * 2f), .1f, Lerp(.5f, 3f, age * maxRadius * 2f));
        transform.localScale = newScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.GetComponentInChildren<AIDamage>() != null)
        {
            if(!other.GetComponentInChildren<AIDamage>().isStunned) enemiesInRange++;
        }
    }

    private float Lerp(float a, float b, float interval)
    {
        return a + (b - a) * interval; 
    }
}
