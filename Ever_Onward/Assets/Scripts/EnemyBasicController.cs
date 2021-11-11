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

        /////////////////// Child Classes

        public class Idle : State
        {
            public override State Update()
            {
                //enemy.myNavMeshAgent.speed = 2.5f;
                //if (enemy.health <= 5) return new States.Attack3();
                if (enemy.enemySeen && enemy.isRangeEnemy || enemy.enemySeen && enemy.isRangeBoss) return new States.RangeAttack();
                if (enemy.inRange) return new States.MeleeAttack();
                return null;
            }
        }

        public class RangeAttack : State
        {
            public override State Update()
            {
                //Behaviour
                if (enemy.isRangeEnemy == true || enemy.isRangeBoss == true)
                {
                    enemy.SpawnProjectile();
                }
                //enemy.myNavMeshAgent.speed = 2.5f;

                //transition
                //if (enemy.health <= 5) return new States.Attack3();
                if (!enemy.enemySeen) return new States.Idle();
                if (enemy.inRange) return new States.MeleeAttack();


                return null;
            }
        }
        
            public class MeleeAttack : State
            {

                public override State Update()
                {
                    //Behaviour
                    
                    // enemy.myNavMeshAgent.speed = 6f;

                    enemy.inRange = false;
                    //transition
                    //if (enemy.healthSystem <= 50) return new States.Attack3();
                    if (!enemy.enemySeen) return new States.Idle();

                    return null;
                }

            }
        /*
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
    private States.State state;
    private NavMeshAgent nav;
    public Transform attackTarget;

    private float healingSpellCooldown = 0;
    private float attackCooldown = 0f;
    private bool enemySeen = false;

    //health and mana 
    
    public float health = 10f;
    public float healthSystem;

    public float mana = 10;
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

    public GameObject cube;
    private Material thisMat;
    public Material thatMat;
    Material[] currentlyAssignedMaterials;
    public Material[] thoseMats;
    Material[] theseMats;
    Renderer rend;

    private float siphonStunTimer = 3f;
    public bool isStunned = false;
    private float saveSpeed;

    public bool isRangeEnemy = false;
    public bool isMeleeEnemy = false;
    public bool isMeleeBoss = false;
    public bool isRangeBoss = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            myNavMeshAgent = GetComponent<NavMeshAgent>();
        }
        myTransform = transform;

        wanderCheckRate = Random.Range(.0005f, .005f);
        headCheckRate = Random.Range(.0005f, .005f);

        rend = GetComponent<Renderer>();
        thisMat = GetComponent<Renderer>().material;
        currentlyAssignedMaterials = GetComponent<Renderer>().materials;
        theseMats = GetComponent<Renderer>().materials;

    }

    void Update()
    {
       
        if (Time.time > wanderNextCheck)
        {
            CheckIfIShouldWander();
            wanderNextCheck = Time.time + wanderCheckRate;
        }
        if (state == null) SwitchState(new States.Idle());
        if (state != null) SwitchState(state.Update());

        ManaRegen();
        if (manaRegenTimer >= 0) manaRegenTimer -= Time.deltaTime;
        if (healingSpellCooldown >= 0) healingSpellCooldown -= Time.deltaTime;
        if (attackCooldown >= 0) attackCooldown -= Time.deltaTime;

        CarryOutDetection();
        headCheckRate = Random.Range(.8f, 1.2f);
        if (headTransform == null) headTransform = myTransform;

        if (isMeleeEnemy == true || isMeleeBoss == true)
        {
            if (myTarget != null) myNavMeshAgent.SetDestination(myTarget.position);
        }

        if (isStunned)
        {
            if (GetComponentInParent<NavMeshAgent>().speed != 0) saveSpeed = GetComponentInParent<NavMeshAgent>().speed;

            GetComponentInParent<NavMeshAgent>().speed = 0;
            siphonStunTimer -= Time.deltaTime;
            if (siphonStunTimer <= 0)
            {
                GetComponentInParent<NavMeshAgent>().speed = saveSpeed;
                isStunned = false;
                siphonStunTimer = 3f;

            }
        }

    }

    void SwitchState(States.State newState)
    {
        if (newState == null) return;

        if (state != null) state.OnEnd();
        state = newState;
        state.OnStart(this);
    }

    void CheckIfIShouldWander()
    {

        if (myTarget == null && !isOnRoute)
        {

            if (RandomWanderTarget(myTransform.position, wanderRange, out wanderTarget))
            {
                myNavMeshAgent.SetDestination(wanderTarget);
                isOnRoute = true;
                myNavMeshAgent.speed = 3.5f;
            }
        }
        else if (isOnRoute)
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
        if (NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, NavMesh.AllAreas))
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



    //boolean that lets the enemy know if they can see the player
    bool CanSeeTarget(Transform potentialTarget)
    {
        if (Physics.Linecast(headTransform.position, potentialTarget.position, out hitTarget, sightLayer))
        {
            if (hitTarget.transform == potentialTarget)
            {
                myTarget = potentialTarget;
                enemySeen = true;
                return true;
            }
            else
            {
                enemySeen = false;
                myTarget = null;
                return false;
            }
        }
        else
        {
            enemySeen = false;
            myTarget = null;
            return false;
        }
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
                        //myTarget = potentialTargetCollider.transform;
                        
                        break;
                    }
                }
            }
            else
            {
                enemySeen = false;
                inRange = false;
                myTarget = null;
                
            }
        }
    }

    //mana regeneration over time when the player/boss uses spells 
    public void ManaRegen()
    {
        if (mana <= 9)
        {
            if (manaRegenTimer <= 0)
            {
                mana += 2f;
                manaRegenTimer = .2f;
            }
        }
        if (mana == 10) return;
    }

    //collision detection between the players bullets and the aoe
    public void OnTriggerEnter(Collider other)
    {
        if (this.tag == ("Enemy") & other.tag == ("Bullet"))
        {
            TakeDamage(5);
        }

        if (other.tag == "Wind")
        {
            health--;

            GetComponent<Renderer>().materials = thoseMats;
            rend.materials = thoseMats;
            if (health <= 0)
            {
                for (int i = 50; i > 0; i--)
                {
                    Instantiate(cube, this.transform.position, this.transform.rotation);
                }
                Destroy(gameObject);
            }

        }

        if (other.tag == "Bramble")
        {
            health -= 3;
            GetComponent<Renderer>().materials = thoseMats;
            rend.materials = thoseMats;
            if (health <= 0)
            {
                for (int i = 50; i > 0; i--)
                {
                    Instantiate(cube, this.transform.position, this.transform.rotation);
                }
                Destroy(gameObject);
            }
        }

        if (other.tag == "Siphon")
        {
            health--;
            GetComponent<Renderer>().materials = thoseMats;
            rend.materials = thoseMats;
            if (health <= 0)
            {
                for (int i = 50; i > 0; i--)
                {
                    Instantiate(cube, this.transform.position, this.transform.rotation);
                }
                Destroy(gameObject);
            }
            isStunned = true;
            siphonStunTimer = 3f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wind" || other.tag == "Bramble" || other.tag == "Siphon")
        {
            GetComponent<Renderer>().materials = theseMats;
            rend.materials = theseMats;
        }
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

                if (mana >= 10)
                {

                    Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
                    p.InitBullet(transform.forward * 20);

                    mana -= 1;
                    manaRegenTimer = 1f;
                    attackCooldown = .75f;
                }
            }
            if (attackCooldown >= 0.1) return;
            if (mana <= 0) return;

        }

    }
