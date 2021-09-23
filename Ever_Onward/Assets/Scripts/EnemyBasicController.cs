using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    public class EnemyBasicController : MonoBehaviour
    {

        static class States
        {
            public class State
            {
                protected EnemyBasicController enemy;
                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(EnemyBasicController enemy)
                {
                    this.enemy = enemy;
                }
                virtual public void OnEnd()
                {

                }
            }

            /// ///////////////// Child Classes

            public class Idle : State
            {
                public override State Update()
                {
                    //enemy.myNavMeshAgent.speed = 2.5f;
                    //if (enemy.healthSystem <= 50) return new States.Attack3();
                    if (enemy.enemySeen) return new States.Attack1();
                    //if (enemy.inRange) return new States.Attack2();
                    return null;
                }
            }
     
            public class Attack1 : State
            {
                public override State Update()
                {
                    //Behaviour
                    enemy.SpawnProjectile();

                    //enemy.myNavMeshAgent.speed = 2.5f;

                    //transition
                    //if (enemy.healthSystem <= 50) return new States.Attack3();
                    if (!enemy.enemySeen) return new States.Idle();
                    //if (enemy.inRange) return new States.Attack2();


                    return null;
                }
            }
        /*
            public class Attack2 : State
            {

                public override State Update()
                {
                    //Behaviour
                    enemy.SpawnAOE();
                    // enemy.myNavMeshAgent.speed = 6f;

                    enemy.inRange = false;
                    //transition
                    if (enemy.healthSystem <= 50) return new States.Attack3();
                    if (!enemy.enemySeen) return new States.Idle();

                    return null;
                }

            }
            public class Attack3 : State
            {
                public override State Update()
                {
                    enemy.SpawnHealthEffect();

                    //enemy.myNavMeshAgent.speed = 6;

                    if (enemy.healthSystem >= 75) return new States.Idle();
                    return null;
                }
            }
        */
        }


        public Projectile prefabProjectile;
        //public AOE damageCircle;
        private States.State state;
        private NavMeshAgent nav;
        public Transform attackTarget;

        private float healingSpellCooldown = 0;
        private float attackCooldown = 0f;
        private bool enemySeen = false;

        //health and mana 
        public static float health { get; set; }
        public float healthMax = 100f;
        public float healthSystem;

        public static float mana { get; set; }
        public float manaMax = 100;
        public float manaRegenTimer = 0f;


        //Enemy Detection
        public Transform headTransform;
        private float headCheckRate;
        private float headNextCheck;
        public float headDetectRaduis = 8;
        private RaycastHit hitTarget;

        //general AI stuff
        public bool isOnRoute;
        public bool isNavPaused;
        public PlayerMovement PM;
        public Transform myTarget;
        private NavMeshHit navHit;
        public float wanderRange = 20;
        private Vector3 wanderTarget;
        private float wanderCheckRate;
        private float wanderNextCheck;

        //Layer Masks
        public LayerMask playerLayer;
        public LayerMask sightLayer;

        private NavMeshAgent myNavMeshAgent;
        private Transform myTransform;

        private bool inRange;

        // Start is called before the first frame update
        void Start()
        {
        if (GetComponent<NavMeshAgent>() != null)
        {
            myNavMeshAgent = GetComponent<NavMeshAgent>();
        }
        myTransform = transform;

            health = healthMax;
            mana = manaMax;
            wanderCheckRate = Random.Range(.01f, .1f);
    }

        void Update()
        {
            if(Time.time > wanderNextCheck)
            {
                CheckIfIShouldWander();
                wanderNextCheck = Time.time + wanderCheckRate;
            }

            if (health >= 100) health = 100;

           healthSystem = health;
           ManaRegen();
           if (manaRegenTimer >= 0) manaRegenTimer -= Time.deltaTime;
           if (healingSpellCooldown >= 0) healingSpellCooldown -= Time.deltaTime;
           if (attackCooldown >= 0) attackCooldown -= Time.deltaTime;

            CarryOutDetection();
            headCheckRate = Random.Range(.8f, 1.2f);
            if (headTransform == null) headTransform = myTransform;

            if (state == null) SwitchState(new States.Idle());
            if (state != null) SwitchState(state.Update());
            if (myTarget != null) myNavMeshAgent.SetDestination(myTarget.position);
        }
        
        void CheckIfIShouldWander()
        {
        
            if(myTarget == null && !isOnRoute && !isNavPaused)
            {
           
                if(RandomWanderTarget(myTransform.position, wanderRange, out wanderTarget))
                {
                print("hello krieg");
                    myNavMeshAgent.SetDestination(wanderTarget);
                    isOnRoute = true;
                    myNavMeshAgent.speed = 3.5f;
                }
            }
            else if(isOnRoute)
            {
                if (myNavMeshAgent.remainingDistance < myNavMeshAgent.stoppingDistance)
                {
                    isOnRoute = false;
                }
            }
    }

    
        bool RandomWanderTarget(Vector3 centre, float range, out Vector3 result)
        {
            Vector3 randomPoint = centre + Random.insideUnitSphere * wanderRange;
            if(NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, NavMesh.AllAreas)) 
            {
                result = navHit.position;
                return true;
            }
            else
            {
                result = centre;
                return false;
            }
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;

            if (state != null) state.OnEnd();
            state = newState;
            state.OnStart(this);
        }

        //boolean that lets the enemy know if they can see the player
        bool CanSeeTarget(Transform potentialTarget)
        {
            if (Physics.Linecast(headTransform.position, potentialTarget.position, out hitTarget, sightLayer))
            {
                if (hitTarget.transform == potentialTarget)
                {
                    SetTarget(potentialTarget);
                    enemySeen = true;
                    return true;
                }
                else
                {
                    enemySeen = false;
                    return false;
                }
            }
            else
            {
                enemySeen = false;
                return false;
            }
        }

        //sets the target
        public void SetTarget(Transform targetTransform)
        {
            myTarget = targetTransform;
        }

        //looks for the enemy by shooting out a raycast
        void CarryOutDetection()
        {
            if (Time.time > headNextCheck)
            {
                headNextCheck = Time.time + headCheckRate;

                Collider[] colliders = Physics.OverlapSphere(myTransform.position, headDetectRaduis, playerLayer);

                if (colliders.Length > 0)
                {
                    foreach (Collider potentialTargetCollider in colliders)
                    {
                        if (CanSeeTarget(potentialTargetCollider.transform))
                        {
                            inRange = true;
                            myTarget = potentialTargetCollider.transform;
                        
                            break;
                        }
                    }
                }

            }
            else
            {
                enemySeen = false;
                myTarget = null;
            }
        }

        //mana regeneration over time when the player/boss uses spells 
        public void ManaRegen()
        {
            if (mana <= 99)
            {
                if (manaRegenTimer <= 0)
                {
                    mana += 2f;
                    manaRegenTimer = .2f;
                }
            }
            if (mana == 100) return;
        }

        //collision detection between the players bullets and the aoe
        public void OnTriggerEnter(Collider other)
        {
            if (this.tag == ("Enemy") & other.tag == ("Bullet"))
            {
                TakeDamage(5);
            }
            /*
            if (this.tag == ("Enemy") & other.tag == ("DamageCircle"))
            {
                health++;
            }
            */
        }

        public void TakeDamage(int amt)
        {
            if (amt <= 0) return;
            health -= amt;

            if (health <= 0) Die();
        }
        public void Die()
        {
            print("DEAD");
            Destroy(gameObject);

        }


        void SpawnProjectile()
        {
            if (attackCooldown <= 0)
            {
               // print("Hello");
                if (mana >= 10)
                {
                   // print("World");
                    Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
                    p.InitBullet(transform.forward * 20);

                    mana -= 10;
                    manaRegenTimer = 1f;
                    attackCooldown = .75f;
                }
            }
            if (attackCooldown >= 0.1) return;
            if (mana <= 0) return;

        }
    /*
        void SpawnAOE()
        {
            if (attackCooldown <= 0)
            {
                print("Hello");
                if (mana >= 15)
                {
                    print("World");
                    AOE a = Instantiate(damageCircle, transform.position, Quaternion.identity);


                    mana -= 15;
                    manaRegenTimer = .1f;
                    attackCooldown = .75f;
                }
            }
            if (attackCooldown >= 0.1) return;
            if (mana <= 0) return;

        }

        void SpawnHealthEffect()
        {
            if (healingSpellCooldown <= 0)
            {
                healingSpellCooldown = 5f;
                if (health <= 100) health += 25;
                if (health >= 100) return;
                print("Healed");
                mana -= 25;
                manaRegenTimer = .1f;
                if (health >= 100) health = 100;
            }
            if (healingSpellCooldown >= 1) return;

        }
    */
    }
