using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace S3
{
    public class gameManagerReferences : MonoBehaviour
    {
        public string playerTag;
        public static string playerTag2;

        public string enemyTag;
        public static string enemyTag2;

        public static GameObject player2;
       
        void OnEnable()
        {
            if(playerTag == "")
            {
                print ("Please type in the name of the player tag in the gameManagerReferences" +
                    "slot in the inspector or else the GTGD S3 systems will not work.");
            }

            if (enemyTag == "")
            {
                print ("Please type in the name of the enemy tag in the gameManagerReferences" +
                    "slot in the inspector or else the GTGD S3 systems will not work.");
            }

            playerTag2 = playerTag;
            enemyTag2 = enemyTag;

            player2 = GameObject.FindGameObjectWithTag(playerTag2);
        }
    }

}

