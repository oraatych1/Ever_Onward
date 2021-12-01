using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndManaSystem : MonoBehaviour
{
    
    public static int health { get; set; }
    public int healthMax = 6;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public static float manaRegenTimer = 0f;

    public static int mana { get; set; }
    public int manaMax = 6;
    public Image mana1;
    public Image mana2;
    public Image mana3;
    public Sprite manaFull;
    public Sprite manaHalf;
    public Sprite manaEmpty;
    public Vector3 respawnPosition1;
    GameObject respawnPoint;


    private void Start()
    {
        health = healthMax;
        mana = manaMax;
        respawnPoint = GameObject.Find("RespawnPoint1");
        respawnPosition1 = transform.position;
    }
    public void Update()
    {
        if (transform.position.y <= -250)
        {
            //print("work fcker");
            //print("heres yer position " + transform.position.ToString());
            //print("respawn here " + respawnPosition1);
            transform.position = respawnPosition1;
        }
        //print(mana);
        ManaRegen();
        if (manaRegenTimer >= 0) manaRegenTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F)) health--;

        heart1.sprite = (health == 0) ? heartEmpty : (health == 1) ? heartHalf : heartFull;
        heart2.sprite = (health <= 2) ? heartEmpty : (health == 3) ? heartHalf : heartFull;
        heart3.sprite = (health <= 4) ? heartEmpty : (health == 5) ? heartHalf : heartFull;

        mana1.sprite = (mana == 0) ? manaEmpty : (mana == 1) ? manaHalf : manaFull;
        mana2.sprite = (mana <= 2) ? manaEmpty : (mana == 3) ? manaHalf : manaFull;
        mana3.sprite = (mana <= 4) ? manaEmpty : (mana == 5) ? manaHalf : manaFull;

        if (mana < 0) mana = 0;
        if (mana > manaMax) mana = manaMax;

        if (health < 0) health = 0;
        if (health > healthMax) health = healthMax;

    }
    public void UseMana(int amt)
    {

        if (amt <= 0) return;

        mana -= amt;



    }


    public void TakeDamage(int amt)
    {
        if (amt <= 0) return;


        health -= amt;

        if (health <= 0) Die();
    }

    public void ManaRegen()
    {


        if (mana <= 5)
        {
            if (manaRegenTimer <= 0)
            {
                mana++; 
                manaRegenTimer = 3f;
            }
        }

        else return;

    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        //print ("Colliding");
        if(other.tag == "RespawnPlane")
        {
            print("Colliding Bottom");
            
        }
    }

}