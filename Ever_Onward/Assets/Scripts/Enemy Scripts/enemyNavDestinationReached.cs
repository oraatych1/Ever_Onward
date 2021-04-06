using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace S3
{
    public class enemyNavDestinationReached : MonoBehaviour
    {
        private enemyMaster myEnemyMaster;
        private NavMeshAgent myNavMeshAgent;
        private float checkRate;
        private float nextCheck;

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                CheckIfDestinationReached();
            }
        }
        void OnEnable()
        {
            SetInitialReferences();
        }
        void SetInitialReferences()
        {
            myEnemyMaster = GetComponent<enemyMaster>();
            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
            checkRate = Random.Range(0.3f, 0.4f);
        }
        void CheckIfDestinationReached()
        {
            if (myEnemyMaster.isOnRoute)
            {
                if(myNavMeshAgent.remainingDistance < myNavMeshAgent.stoppingDistance)
                {
                    myEnemyMaster.isOnRoute = false;
                    myEnemyMaster.CallEventEnemyReachedNavTarget();
                    enemyText.enemyState = 0;
                }
            }
        }
        void DisableThis()
        {
            this.enabled = false;
        }
    }
}
