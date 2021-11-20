using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpellSwitches : MonoBehaviour
{
    public Image activeSpell;
    public Sprite windImg, lightImg, natureImg, whirlwindImg, brambleImg, siphonImg;
    public Projectile WindProjectile;
    public Projectile GrassProjectile;
    public AOE LightAOE, BrambleAOE, SiphonAOE;
    private CharacterController player;
    GameObject spawnPoint;
    //public DialogueSystem dialogue;
   
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


    private float WhirlwindCooldown = 0f;
    private float SeedSiphonCooldown = 0f;
    private float BrambleBlastCooldown = 0f;


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
                    //print("PEW PEW");
                    if (Input.GetButtonDown("Fire1") && !DialogueSystem.inConversation && HealthAndManaSystem.mana > 0)
                    {
                        spell.SpawnWindProjectile();
                        HealthAndManaSystem.mana--;
                        HealthAndManaSystem.manaRegenTimer = 3f;
                    }
                }

                //if (!spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                if (spell.lightPressed) return new States.Light();
                if (spell.whirlwindCombo) return new States.Whirlwind();
                if (spell.seedSiphonCombo) return new States.SeedSiphon();
                if (spell.brambleBlastCombo) return new States.BrambleBlast();

                return null;
                //transitions
            }
        }
        public class Grass : State
        {
            public override State Update()
            {
                //behavoir
                if(spell.grassPressed)
                {
                    
                    if (Input.GetButtonDown("Fire1") && !DialogueSystem.inConversation && HealthAndManaSystem.mana > 0)
                    {
                        print("GRASS SPELL");
                        spell.SpawnGrassProjectile();
                        HealthAndManaSystem.mana--;
                        //HealthAndManaSystem.manaRegenTimer = 3f;
                    }

                }
                

                if (spell.windPresssed) return new States.Wind();
                if (spell.lightPressed) return new States.Light();
                if (spell.whirlwindCombo) return new States.Whirlwind();
                if (spell.seedSiphonCombo) return new States.SeedSiphon();
                if (spell.brambleBlastCombo) return new States.BrambleBlast();
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
                    //print("LIGHT CLASS");
                    if (Input.GetButtonDown("Fire1") && !DialogueSystem.inConversation && HealthAndManaSystem.mana > 0 && HealthAndManaSystem.health < 6)
                    {
                        spell.SpawnAOE("light");
                        HealthAndManaSystem.mana--;
                        HealthAndManaSystem.manaRegenTimer = 3f;
                        HealthAndManaSystem.health++;
                    }
                }

                if (spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                if (spell.whirlwindCombo) return new States.Whirlwind();
                if (spell.seedSiphonCombo) return new States.SeedSiphon();
                if (spell.brambleBlastCombo) return new States.BrambleBlast();
                return null;
                //transitions
            }
        }
        
        public class Whirlwind : State
        {

            public float WhirlwindCooldown = 0f;

            public override State Update()
            {
                WhirlwindCooldown -= Time.deltaTime;
                //behavoir
                if (spell.whirlwindCombo)
                {
                    print("Whirlwind CLASS combo");
                    if (Input.GetButtonDown("Fire1") && !DialogueSystem.inConversation && HealthAndManaSystem.mana > 1)
                    {
                        // Find what the player is looking at

                        // Teleport the player there

                        // Take mana cost
                    }


                }

                //if (WhirlwindCooldown <= 0) return new States.Light();
                if (spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                if (spell.lightPressed) return new States.Light();
                if (spell.seedSiphonCombo) return new States.SeedSiphon();
                if (spell.brambleBlastCombo) return new States.BrambleBlast();
                return null;
                //transitions
            }
        }
        public class SeedSiphon : State
        {
            /*
            public float siphonCooldown = 0f;
            public override void OnStart(PlayerSpellSwitches spell)
            {
                
                siphonCooldown = 1f;
            }
            */

            public override State Update()
            {
                
                //siphonCooldown -= Time.deltaTime;
                if (spell.seedSiphonCombo)
                {
                    
                    if (Input.GetButtonDown("Fire1") && !DialogueSystem.inConversation && HealthAndManaSystem.mana > 1)
                    {
                        // Summon AOE
                        spell.SpawnAOE("siphon");
                        // Take mana cost
                        HealthAndManaSystem.mana -= 2;
                        HealthAndManaSystem.manaRegenTimer = 3f;
                        
                        // Damage and freeze all enemies in AOE (DONE)

                        // Find number of enemies hit by AOE

                        // For each enemy hit, restore 1 health


                    }
                }
                //if (siphonCooldown <= 0) return new States.Grass();
                if (spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                if (spell.lightPressed) return new States.Light();
                if (spell.whirlwindCombo) return new States.Whirlwind();
                if (spell.brambleBlastCombo) return new States.BrambleBlast();
                return null;
            }
        }

        public class BrambleBlast : State
        {


            public override State Update()
            {

                //brambleCooldown -= Time.deltaTime;
                print("Blam!");
               // if (spell.brambleBlastCombo)
              //  {
                    if (Input.GetButtonDown("Fire1") && !DialogueSystem.inConversation && HealthAndManaSystem.mana > 1)
                    {
                        
                        spell.SpawnAOE("bramble");
                        HealthAndManaSystem.mana-=2;
                        HealthAndManaSystem.manaRegenTimer = 3f;
                   
                    }
               // }
                //if (brambleCooldown <= 0) return new States.Wind();
                if (spell.windPresssed) return new States.Wind();
                if (spell.grassPressed) return new States.Grass();
                if (spell.lightPressed) return new States.Light();
                if (spell.whirlwindCombo) return new States.Whirlwind();
                if (spell.seedSiphonCombo) return new States.SeedSiphon();
                return null;
            }
        }
        
    }

    private States.State state;

    void Start()
    {
        player = GetComponent<CharacterController>();
        spawnPoint = GameObject.Find("spawnpoint");
    }

    // Update is called once per frame
    void Update()
    {

        if (state == null) SwitchState(new States.Wind());
        else SwitchState(state.Update());

        // print(currentSpellState);
        //print(whirlwindCombo);
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
                

                if (Input.GetButton("Shortcut2")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.BrambleBlast : SpellState.Grass;
                if (Input.GetButton("Shortcut3")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.Whirlwind : SpellState.Light;

                break;

            case SpellState.Grass:
                windPresssed = false;
                lightPressed = false;
                grassPressed = true;
                activeSpell.sprite = natureImg;

                whirlwindCombo = false;
                seedSiphonCombo = false;
                brambleBlastCombo = false;

                if (Input.GetButton("Shortcut1")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.BrambleBlast : SpellState.Wind;
                if (Input.GetButton("Shortcut3")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.Whirlwind : SpellState.Light;



                break;

            case SpellState.Light:
                windPresssed = false;
                lightPressed = true;
                grassPressed = false;

                whirlwindCombo = false;
                seedSiphonCombo = false;
                brambleBlastCombo = false;

                activeSpell.sprite = lightImg;
                if (Input.GetButton("Shortcut1")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.BrambleBlast : SpellState.Wind;
                if (Input.GetButton("Shortcut2")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.SeedSiphon : SpellState.Grass;


                break;

            case SpellState.Whirlwind:
                whirlwindCombo = true;
                seedSiphonCombo = false;
                brambleBlastCombo = false;

                windPresssed = false;
                lightPressed = false;
                grassPressed = false;

                activeSpell.sprite = whirlwindImg;

                // if (WhirlwindCooldown <= 0) currentSpellState = SpellState.Wind;
                if (Input.GetButton("Shortcut1")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.BrambleBlast : SpellState.Wind;
                if (Input.GetButton("Shortcut2")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.SeedSiphon : SpellState.Grass;
                if (Input.GetButton("Shortcut3")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.Whirlwind : SpellState.Light;

                break;

            case SpellState.SeedSiphon:
                whirlwindCombo = false;
                seedSiphonCombo = true;
                brambleBlastCombo = false;

                windPresssed = false;
                lightPressed = false;
                grassPressed = false;

                activeSpell.sprite = siphonImg;

                //print("Seeds!");
                // if (SeedSiphonCooldown <= 0) currentSpellState = SpellState.Light;
                if (Input.GetButton("Shortcut1")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.BrambleBlast : SpellState.Wind;
                if (Input.GetButton("Shortcut2")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.SeedSiphon : SpellState.Grass;
                if (Input.GetButton("Shortcut3")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.Whirlwind : SpellState.Light;

                break;

            case SpellState.BrambleBlast:
                whirlwindCombo = false;
                seedSiphonCombo = false;
                brambleBlastCombo = true;

                windPresssed = false;
                lightPressed = false;
                grassPressed = false;

                activeSpell.sprite = brambleImg;
                //print("Brambles!");
                if (Input.GetButton("Shortcut1")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.BrambleBlast : SpellState.Wind;
                if (Input.GetButton("Shortcut2")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.SeedSiphon : SpellState.Grass;
                if (Input.GetButton("Shortcut3")) currentSpellState = (Input.GetKey(KeyCode.LeftShift)) ? SpellState.Whirlwind : SpellState.Light;
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

        Projectile p = Instantiate(WindProjectile, spawnPoint.transform.position, Camera.main.transform.rotation);
        p.InitBullet(Camera.main.transform.forward * 20);

    }

    void SpawnGrassProjectile()
    {
        if (grassCooldown > 0) return;

        Projectile p = Instantiate(GrassProjectile, spawnPoint.transform.position, Camera.main.transform.rotation);
        p.InitBullet(Camera.main.transform.forward * 20);


    }

    
     
    void SpawnAOE(string type)
    {
        switch(type)
        {
            case "light":
                if (lightCooldown > 0) return;
                AOE a = Instantiate(LightAOE, transform.position, Quaternion.identity);
                break;
            case "bramble":
                if (BrambleBlastCooldown > 0) return;
                AOE b = Instantiate(BrambleAOE, transform.position, Quaternion.identity);
                break;
            case "siphon":
                if (SeedSiphonCooldown > 0) return;
                AOE s = Instantiate(SiphonAOE, transform.position, Quaternion.identity);
                break;
        }
        
        
    }

}
