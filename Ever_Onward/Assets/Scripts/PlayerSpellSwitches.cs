using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpellSwitches : MonoBehaviour
{
    public Projectile prefabProjectile;
    public Image activeSpell;
    public Sprite windImg;
    public Sprite lightImg;
    public Sprite natureImg;
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
                activeSpell.sprite = windImg;
                if (Input.GetButton("Shortcut2")) currentSpellState = SpellState.Grass;
                if (Input.GetButton("Shortcut3")) currentSpellState = SpellState.Light;
                break;

            case SpellState.Grass:
                windPresssed = false;
                lightPressed = true;
                grassPressed = false;
                activeSpell.sprite = natureImg;
                if (Input.GetButton("Shortcut1")) currentSpellState = SpellState.Wind;
                if (Input.GetButton("Shortcut3")) currentSpellState = SpellState.Light;
                break;

            case SpellState.Light:
                windPresssed = false;
                lightPressed = false;
                grassPressed = true;
                activeSpell.sprite = lightImg;
                if (Input.GetButton("Shortcut1")) currentSpellState = SpellState.Wind;
                if (Input.GetButton("Shortcut2")) currentSpellState = SpellState.Grass;
                break;
        }

    }
}
