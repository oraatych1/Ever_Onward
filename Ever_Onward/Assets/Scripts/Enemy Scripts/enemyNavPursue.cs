using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace S3
{
    public class enemyNavPursue : MonoBehaviour
    {
        private enemyMaster myEnemyMaster;
        private NavMeshAgent myNavMeshAgent;
        private float checkRate;
        private float nextCheck;
        
        void OnEnable()
        {
            SetInitialReferences();
        }

        void OnDisable()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                TryToChaseTarget();
            }
        }

        void SetInitialReferences()
        {
            myEnemyMaster = GetComponent<enemyMaster>();
            if(GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
            checkRate = Random.Range(0.1f, 0.2f);
        }

        void TryToChaseTarget()
        {
            if(myEnemyMaster.myTarget !=null && myNavMeshAgent !=null && !myEnemyMaster.isNavPaused)
            {
                myNavMeshAgent.SetDestination(myEnemyMaster.myTarget.position);

                if(myNavMeshAgent.remainingDistance > myNavMeshAgent.stoppingDistance)
                {
                    myEnemyMaster.CallEventEnemyWalking();
                    myEnemyMaster.isOnRoute = true;
                    enemyText.enemyState = 3;
                    myNavMeshAgent.speed = 8;
                }
            }
        }

        void DisableThis()
        {
            if(myNavMeshAgent != null)
            {
                myNavMeshAgent.enabled = false;
            }
            this.enabled = false;
        }

    }
}
