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
                else if (enemy.inRange) return new States.MeleeAttack();
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
                    enemy.SpawnProjectile(.75f);
                }
                //enemy.myNavMeshAgent.speed = 2.5f;

                //transition
                if (enemy.health <= 5) return new States.Attack3();
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
                if (enemy.isRangeEnemy == true || enemy.isRangeBoss == true)
                {
                    enemy.SpawnProjectile(.5f);
                }
                
                if (!enemy.enemySeen) return new States.Idle();
                if (!enemy.inRange) return new States.Idle();

                return null;
                }

            }
        
            public class Attack3 : State
            {
                public override State Update()
                {



                    if (enemy.healthSystem >= 75) return new States.Idle();
                    return null;
                }
            }
        
    }


    public Projectile prefabProjectile;
    private States.State state;
    private NavMeshAgent nav;
    public Transform attackTarget;

    private float healingSpellCooldown = 0;
    private float attackCooldown = 0f;
    private bool enemySeen = false;

    //health
    public float health = 10f;
    public float healthSystem;

    public float tester = 20;
    public bool lockedInPlace;

    //Enemy Detection
    public Transform headTransform;
    private float headCheckRate;
    private float headNextCheck;
    public float headDetectRaduis = 8;
    public float meleeDetectRaduis = 2;
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

    //look at 
    private Quaternion startingRotation;

    public bool lockEnemyRotationX;
    public bool lockEnemyRotationY;
    public bool lockEnemyRotationZ;

    //what the enemy is
    public bool isRangeEnemy = false;
    public bool isMeleeEnemy = false;
    public bool isMeleeBoss = false;
    public bool isRangeBoss = false;

    public float alert = 20;
    Vector3 danger;


    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            myNavMeshAgent = GetComponent<NavMeshAgent>();
        }
        myTransform = transform;

        wanderCheckRate = Random.Range(.0005f, .005f);

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
            if (attackCooldown >= 0) attackCooldown -= Time.deltaTime;

            if (state == null) SwitchState(new States.Idle());
            if (state != null) SwitchState(state.Update());

            CarryOutDetection();
            TurnTowardTarget();
            headCheckRate = Random.Range(.8f, 1.2f);
            if (headTransform == null) headTransform = myTransform;

        if (isMeleeEnemy == true || isMeleeBoss == true)
        {
            if (myTarget != null && isMeleeEnemy)
            {
                GetComponentInParent<NavMeshAgent>().speed = 19f;
                myNavMeshAgent.SetDestination(myTarget.position);
            }

            if (myTarget != null && isMeleeBoss)
            {
                GetComponentInParent<NavMeshAgent>().speed = 10f;
                myNavMeshAgent.SetDestination(myTarget.position);
            }
        }
            if (isRangeEnemy == true || isRangeBoss == true)
            {
                if (myTarget != null) myNavMeshAgent.SetDestination(myTransform.position);
            }

            if (isRangeEnemy == true && inRange == true|| isRangeBoss == true && inRange == true)
            {
            print("DANGER");
            print(danger);
                Vector3 randomPoint = myTransform.position + Random.insideUnitSphere * alert;
                if (NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, NavMesh.AllAreas))
                {
                    danger = navHit.position;
                }
            GetComponentInParent<NavMeshAgent>().speed = 23f;
            if (myTarget != null) myNavMeshAgent.SetDestination(danger);
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
        
        if (lockedInPlace == true) myNavMeshAgent.speed = 0;

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
                myNavMeshAgent.speed = 5f;
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
            Collider[] colli = Physics.OverlapSphere(myTransform.position, meleeDetectRaduis, playerLayer);

            //for line of sight on the player
            if (colliders.Length > 0)
            {
                foreach (Collider potentialTargetCollider in colliders)
                {
                    if (CanSeeTarget(potentialTargetCollider.transform))
                    {

                        break;
                    }
                }
            }
            else
            {
                enemySeen = false;
                myTarget = null;
                
            }
            //when player is close to the enemy
            if (colli.Length > 0)
            {
                foreach (Collider potentialTargetCollider in colli)
                {
                    if (CanSeeTarget(potentialTargetCollider.transform))
                    {
                        inRange = true;
                        break;
                    }
                }
            }
            else
            {
                inRange = false;
            }
        }
    }

    void TurnTowardTarget()
    {
        if (myTarget != null)
        {
            Vector3 disToTaget = myTarget.position - myTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(disToTaget, Vector3.up);

            Vector3 euler1 = myTransform.localEulerAngles;
            Quaternion prevRot = myTransform.rotation;
            myTransform.rotation = targetRotation;
            Vector3 euler2 = transform.localEulerAngles;

            if (lockEnemyRotationX) euler2.x = euler1.x;
            if (lockEnemyRotationY) euler2.y = euler1.y;
            if (lockEnemyRotationZ) euler2.z = euler1.z;

            myTransform.rotation = prevRot;
            myTransform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), 0);
        }
        else
        {
            myTransform.localRotation = AnimMath.Slide(myTransform.localRotation, startingRotation, .05f);
        }
    }
    //collision detection between the players bullets and the aoe
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wind")
        {
            health -= 2.5f;

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

    void SpawnProjectile(float speed)
        {
            if (attackCooldown <= 0)
            {

                {
                    Projectile p = Instantiate(prefabProjectile, myTransform.position, myTransform.rotation);
                    p.InitBullet(myTransform.forward * tester);
                    attackCooldown = speed;
                }
            }


            if (attackCooldown >= 0.001) return;
        }

    }
