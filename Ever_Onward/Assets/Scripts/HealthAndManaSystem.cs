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

    public static int mana { get; set; }
    public int manaMax = 6;
    public Image mana1;
    public Image mana2;
    public Image mana3;
    public Sprite manaFull;
    public Sprite manaHalf;
    public Sprite manaEmpty;

    private void Start()
    {
        health = healthMax;
        mana = manaMax;
    }
    public void Update()
    {
        print(mana);
        //if (Input.GetKeyDown(KeyCode.F)) health--;
        switch (health)
        {
            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                return;
            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                return;
            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                return;
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;
        }
        switch (mana)
        {
            case 1:
                mana1.sprite = manaHalf;
                mana2.sprite = manaEmpty;
                mana3.sprite = manaEmpty;
                return;
            case 2:
                mana1.sprite = manaFull;
                mana2.sprite = manaEmpty;
                mana3.sprite = manaEmpty;
                return;
            case 3:
                mana1.sprite = manaFull;
                mana2.sprite = manaHalf;
                mana3.sprite = manaEmpty;
                return;
            case 4:
                mana1.sprite = manaFull;
                mana2.sprite = manaFull;
                mana3.sprite = manaEmpty;
                return;
            case 5:
                mana1.sprite = manaFull;
                mana2.sprite = manaFull;
                mana3.sprite = manaHalf;
                return;
            case 6:
                mana1.sprite = manaFull;
                mana2.sprite = manaFull;
                mana3.sprite = manaFull;
                return;
        }
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


    public void Die()
    {
        Destroy(gameObject);
    }

}