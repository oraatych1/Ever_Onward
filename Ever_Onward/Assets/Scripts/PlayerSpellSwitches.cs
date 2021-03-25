using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellSwitches : MonoBehaviour
{
    public Projectile prefabProjectile;
   
    public enum SpellState
    {
        Wind,
        Grass, 
        Light
    }

    private bool windPresssed = false;
    private bool grassPressed = false;
    private bool lightPressed = false;


    SpellState currentSpellState = SpellState.Wind;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(currentSpellState);
        switch (currentSpellState)
        {
            case SpellState.Wind:
                windPresssed = true;
                lightPressed = false;
                grassPressed = false;

                if (Input.GetButton("Shortcut2")) currentSpellState = SpellState.Grass;
                if (Input.GetButton("Shortcut3")) currentSpellState = SpellState.Light;
                break;

            case SpellState.Grass:
                windPresssed = false;
                lightPressed = true;
                grassPressed = false;

                if (Input.GetButton("Shortcut1")) currentSpellState = SpellState.Wind;
                if (Input.GetButton("Shortcut3")) currentSpellState = SpellState.Light;
                break;

            case SpellState.Light:
                windPresssed = false;
                lightPressed = false;
                grassPressed = true;

                if (Input.GetButton("Shortcut1")) currentSpellState = SpellState.Wind;
                if (Input.GetButton("Shortcut2")) currentSpellState = SpellState.Grass;
                break;
        }

    }
}
