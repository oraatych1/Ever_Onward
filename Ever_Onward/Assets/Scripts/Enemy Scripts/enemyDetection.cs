using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyDetection : MonoBehaviour
{

    private enemyMaster myEnemyMaster;
    private Transform myTransform;
    public Transform head;
    public LayerMask playerLayer;
    public LayerMask sightLayer;
    private float checkRate;
    private float nextCheck;
    public float detectRadius = 10;
    private RaycastHit hit;
    public static bool isCharge = false;

    void OnEnable()
    {
        SetInitialReferences();
        myEnemyMaster.EventEnemyDie += DisableThis;
    }

    void OnDisable()
    {
        myEnemyMaster.EventEnemyDie -= DisableThis;
    }

    void Update()
    {
        CarryOutDetection();
        if (isCharge) GetComponent<NavMeshAgent>().speed = 10000f;
        else GetComponent<NavMeshAgent>().speed = 3.5f;
       // print(GetComponent<NavMeshAgent>().speed);
    }

    void SetInitialReferences()
    {
        myEnemyMaster = GetComponent<enemyMaster>();
        myTransform = transform;

        if (head == null)
        {
            head = myTransform;
        }

        checkRate = Random.Range(0.8f, 1.2f);

    }

    void CarryOutDetection()
    {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;

            Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);


            if(colliders.Length > 0)
            {
                foreach(Collider potentialTargetCollider in colliders)
                {
                    if (potentialTargetCollider.tag == "Player")
                    {
                        if (CanPotentialTargetBeSeen(potentialTargetCollider.transform))
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                myEnemyMaster.CallEventEnemyLostTarget();
            }
        }
    }

    bool CanPotentialTargetBeSeen(Transform potentialTarget)
    {
        if (Physics.Linecast(head.position, potentialTarget.position, out hit, sightLayer))
        {
            if (hit.transform == potentialTarget)
            {
                myEnemyMaster.CallEventEnemySetNavTarget(potentialTarget);
                return true;
                //print("SEEN");
            }
            else
            {
                myEnemyMaster.CallEventEnemyLostTarget();
                return false;
                //print ("LOST");
            }
        }
        else
        {
            myEnemyMaster.CallEventEnemyLostTarget();
            return false;
        }
    }

    void DisableThis()
    {
        this.enabled = false;
    }

}
