using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace S3
{
    public class enemyNavPause : MonoBehaviour
    {
        private enemyMaster myEnemyMaster;
        private NavMeshAgent myNavMeshAgent;
        private float pauseTime = 1;

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
        }
        void PauseNavMeshAgent()
        {
            if(myNavMeshAgent != null)
            {
                if (myNavMeshAgent.enabled)
                {
                    myNavMeshAgent.ResetPath();
                    myEnemyMaster.isNavPaused = true;
                    StartCoroutine("RestartNavMeshAgent()");
                }
            }

        }

        IEnumerator RestartNavMeshAgent()
        {
            yield return new WaitForSeconds(pauseTime);
            myEnemyMaster.isNavPaused = false;
        }
    }
}

