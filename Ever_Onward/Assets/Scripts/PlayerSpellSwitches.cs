using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpellSwitches : MonoBehaviour
{
    public Image activeSpell;
    public Sprite windImg;
    public Sprite lightImg;
    public Sprite natureImg;
    public Projectile WindProjectile;
    public AOE LightAOE;
    private CharacterController player;
   
    public enum SpellState
    {
        Wind,
        Grass, 
        Light,
        Whirlwind,
        SeedSiphon,
        BrambleBlast

    }

    private bool windPresssed = false;
    private bool grassPressed = false;
    private bool lightPressed = false;

    private bool whirlwindCombo = false;
    private bool seedSiphonCombo = false;
    private bool brambleBlastCombo = false;

    private float windCooldown = 0f;
    private float grassCooldown = 0f;
    private float lightCooldown = 0f;

/*
    private float WhirlwindCooldown = 0f;
    private float SeedSiphonCooldown = 0f;
    private float BrambleBlastCooldown = 0f;
*/

    SpellState currentSpellState = SpellState.Wind;

    static class States
    {
        public class State
        {
            protected PlayerSpellSwitches spell;
            virtual public State Update()
            {
                return null;
            }

            virtual public void OnStart(PlayerSpellSwitches spell)
            {
                this.spell = spell;
            }
            virtual public void OnEnd()
            {

            }
        }
        public class Wind : State
        {
            public override State Update()
            {
                //behavoir
                if (spell.windPresssed)
                {
                    print("PEW PEW");
                    if (Input.GetButtonDown("Fire1")) spell.SpawnWindProjectile();
                }

                //if (!spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                if (spell.lightPressed) return new States.Light();
                //if (spell.whirlwindCombo) return new States.Whirlwind();
                return null;
                //transitions
            }
        }
        public class Grass : State
        {
            public override State Update()
            {
                //behavoir
                if (spell.grassPressed)
                {
                    print("GRASS SPELL");
                    // if (Input.GetButtonDown("Fire1")) spell.SpawnWindProjectile();
                }

                if (spell.windPresssed) return new States.Wind();
                if (spell.lightPressed) return new States.Light();
                return null;
                //transitions
            }
        }
        public class Light : State
        {
            public override State Update()
            {
                //behavoir
                if (spell.lightPressed)
                {
                    print("LIGHT CLASS");
                     if (Input.GetButtonDown("Fire1")) spell.SpawnLightAOE();
                }

                if (spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                return null;
                //transitions
            }
        }
        /*
        public class Whirlwind : State
        {
            public float WhirlwindCooldown = 0f;
            public override void OnStart(PlayerSpellSwitches spell)
            {
                WhirlwindCooldown = 5;
            }
            public override State Update()
            {
                WhirlwindCooldown--;
                //behavoir
                if (spell.whirlwindCombo)
                {
                    print("Whirlwind CLASS combo");
                    
                }

                if (WhirlwindCooldown <= 0) return new States.Wind();
                if (spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                if (spell.lightPressed) return new States.Light();
                return null;
                //transitions
            }
        }
        */
    }

    private States.State state;

    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == null) SwitchState(new States.Wind());
        if (state != null) SwitchState(state.Update());

        // print(currentSpellState);
        print(whirlwindCombo);
        switch (currentSpellState)
        {
            case SpellState.Wind:
                windPresssed = true;
                lightPressed = false;
                grassPressed = false;
                activeSpell.sprite = windImg;

                whirlwindCombo = false;
                seedSiphonCombo = false;
                brambleBlastCombo = false;

                if (Input.GetButton("Shortcut2")) currentSpellState = SpellState.Grass;
                if (Input.GetButton("Shortcut3")) currentSpellState = SpellState.Light;

                //combos
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Shortcut3")) currentSpellState = SpellState.Whirlwind;
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Shortcut2")) currentSpellState = SpellState.BrambleBlast;

                break;

            case SpellState.Grass:
                windPresssed = false;
                lightPressed = true;
                grassPressed = false;
                activeSpell.sprite = natureImg;

                whirlwindCombo = false;
                seedSiphonCombo = false;
                brambleBlastCombo = false;

                if (Input.GetButton("Shortcut1")) currentSpellState = SpellState.Wind;
                if (Input.GetButton("Shortcut3")) currentSpellState = SpellState.Light;

                //combos
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Shortcut3")) currentSpellState = SpellState.SeedSiphon;
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Shortcut1")) currentSpellState = SpellState.BrambleBlast;



                break;

            case SpellState.Light:
                windPresssed = false;
                lightPressed = false;
                grassPressed = true;

                whirlwindCombo = false;
                seedSiphonCombo = false;
                brambleBlastCombo = false;

                activeSpell.sprite = lightImg;
                if (Input.GetButton("Shortcut1")) currentSpellState = SpellState.Wind;
                if (Input.GetButton("Shortcut2")) currentSpellState = SpellState.Grass;

                //combos
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Shortcut2")) currentSpellState = SpellState.SeedSiphon;
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Shortcut1")) currentSpellState = SpellState.BrambleBlast;

                break;

            case SpellState.Whirlwind:
                whirlwindCombo = true;
                seedSiphonCombo = false;
                brambleBlastCombo = false;

                windPresssed = false;
                lightPressed = false;
                grassPressed = false;

               // if (WhirlwindCooldown <= 0) currentSpellState = SpellState.Wind;

                break;

            case SpellState.SeedSiphon:
                whirlwindCombo = false;
                seedSiphonCombo = true;
                brambleBlastCombo = false;

                windPresssed = false;
                lightPressed = false;
                grassPressed = false;

               // if (SeedSiphonCooldown <= 0) currentSpellState = SpellState.Light;

                break;

            case SpellState.BrambleBlast:
                whirlwindCombo = false;
                seedSiphonCombo = false;
                brambleBlastCombo = true;

                windPresssed = false;
                lightPressed = false;
                grassPressed = false;

              // if (BrambleBlastCooldown <= 0) currentSpellState = SpellState.Grass;

                break;
        }

    }

    void SwitchState(States.State newState)
    {
        if (newState == null) return;
        if (state != null) state.OnEnd();

        state = newState;
        state.OnStart(this);

    }

    void SpawnWindProjectile()
    {
        if (windCooldown > 0) return;

        Projectile p = Instantiate(WindProjectile, transform.position, Quaternion.identity);
        p.InitBullet(transform.forward * 20);

    }
     
    void SpawnLightAOE()
    {
        if (lightCooldown > 0) return;

        AOE a = Instantiate(LightAOE, transform.position, Quaternion.identity);
        
    }
}
