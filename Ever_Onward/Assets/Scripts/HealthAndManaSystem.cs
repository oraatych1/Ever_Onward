using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndManaSystem : MonoBehaviour
{

    public float health { get; private set; }
    public float healthMax = 100;
    public Scrollbar healthbar;

    public float mana { get; private set; }
    public float manaMax = 100;
    public Scrollbar manabar;

    private void Start()
    {
        health = healthMax;
        mana = manaMax;
    }
    public void Update()
    {
        healthbar.size = health/100;
        manabar.size = mana/100;
    }
    public void UseMana(float amt)
    {

        if (amt <= 0) return;

        mana -= amt;



    }


    public void TakeDamage(float amt)
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