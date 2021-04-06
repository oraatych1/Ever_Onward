using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyCharge : MonoBehaviour
{
    private enemyMaster myEnemyMaster;
    private Transform myTransform;
    public Transform head;
    public Transform AI;
    public LayerMask playerLayer;
    public LayerMask sightLayer;
    private float checkRate;
    private float nextCheck;
    public float detectRadius = 10;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        // sightPill.GetComponent<SphereCollider>();   
        myEnemyMaster = GetComponent<enemyMaster>();
        myTransform = transform;
        //print(enemyMaster);
        if (head == null)
        {
            head = myTransform;
        }

        checkRate = Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(AI.position.x, AI.position.y, AI.position.z);
        this.transform.eulerAngles = new Vector3(AI.rotation.x, AI.eulerAngles.y, AI.rotation.z);
        /*
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;

            Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);


            if (colliders.Length > 0)
            {
                foreach (Collider potentialTargetCollider in colliders)
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
        */
    }
    bool CanPotentialTargetBeSeen(Transform potentialTarget)
    {
        if (Physics.Linecast(head.position, potentialTarget.position, out hit, sightLayer))
        {
            if (hit.transform == potentialTarget)
            {
                myEnemyMaster.CallEventEnemySetNavTarget(potentialTarget);
                return true;
            }
            else
            {
                myEnemyMaster.CallEventEnemyLostTarget();
                return false;
            }
        }
        else
        {
            myEnemyMaster.CallEventEnemyLostTarget();
            return false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyDetection.isCharge = true;
            enemyText.enemyState = 4;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyDetection.isCharge = false;
        }
    }
}
