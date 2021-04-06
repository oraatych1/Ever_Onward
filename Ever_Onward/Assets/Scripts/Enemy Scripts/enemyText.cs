using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyText : MonoBehaviour
{
    public static int enemyState;
    public Text myEnemyText;
    public Text distanceText;
    public Transform AI;
    public Transform player;
    public Vector3 distance = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.position = new Vector3(AI.position.x + 1, AI.position.y + 1, AI.position.z + 1);
        switch (enemyState)
        {
            case 4:
                myEnemyText.text = ("ATTACK");
                break;
            case 3:
                myEnemyText.text = ("PURSUE");
                break;
            case 2:
                myEnemyText.text = ("DODGE");
                break;
            case 1:
                myEnemyText.text = ("PATROL");
                break;
            case 0:
                myEnemyText.text = ("IDLE");
                break;
        }
        Vector3 origin = player.position;
        Vector3 target = this.transform.position;
        float t = 1f;
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        //Vector3 vector = new Vector3(vx, vy, vz);

        if (vx < 0) vx *= -1;
        if (vz < 0) vz *= -1;
        float distance = Mathf.Pow(vx, 2f) + Mathf.Pow(vz, 2f);
        distance = Mathf.Sqrt(distance) - 1;
        //this.transform.position = new Vector3(player.position.x, player.position.y, player.position.z + 1);
        distanceText.text = Mathf.Floor(distance).ToString();

    }
}
