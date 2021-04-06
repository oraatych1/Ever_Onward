using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace S3
{
    public class enemyNavWander : MonoBehaviour
    {
        private enemyMaster myEnemyMaster;
        private NavMeshAgent myNavMeshAgent;
        private float checkRate;
        private float nextCheck;
        private float wanderRange = 15;
        private Transform myTransform;
        private NavMeshHit navHit;
        private Vector3 wanderTarget;


        void OnEnable()
        {
            SetInitialReferences();
        }
        void Update()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                CheckIfIShouldWander();
            }
        }
        void SetInitialReferences()
        {
            myEnemyMaster = GetComponent<enemyMaster>();
            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
            checkRate = Random.Range(0.3f, 0.4f);
            myTransform = transform;
        }
        void CheckIfIShouldWander()
        {
            if (myEnemyMaster.myTarget == null && !myEnemyMaster.isOnRoute && !myEnemyMaster.isNavPaused)
            {
                if(RandomWanderTarget(myTransform.position, wanderRange, out wanderTarget))
                {
                    myNavMeshAgent.SetDestination(wanderTarget);
                    myEnemyMaster.isOnRoute = true;
                    myEnemyMaster.CallEventEnemyWalking();
                    enemyText.enemyState = 1;
                }
            }
        }

        bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if(NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }
            else
            {
                result = center;
                return false;
            }
        }
        
        void DisableThis()
        {
            this.enabled = false;
        }
    }
}

