using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))] //tells unity this script needs a camera to work
public class Raycasting : MonoBehaviour
{
    private Camera cam;
    public DialogueSystem dialogueSystem;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();


    }

    // Update is called once per frame
    void Update()
    {
        //did the user click on this game tick?
        if (cam != null && Input.GetButtonDown("Fire1"))
        {
            //shoot a ray into the scene

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            //????
            //draw ray
            Debug.DrawRay(ray.origin, ray.direction, Color.black, .5f);

            if (Physics.Raycast(ray, out hit))
            {
                //raycast hit controller in scene

                if (dialogueSystem.inConversation == false)
                {
                    DialogueTrigger dialogueTrigger = hit.transform.GetComponent<DialogueTrigger>();
                    if (dialogueTrigger != null) dialogueTrigger.TriggerDialogue();
                }




            }

        }
    }
}
