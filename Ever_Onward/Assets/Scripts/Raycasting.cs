using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))] //tells unity this script needs a camera to work
public class Raycasting : MonoBehaviour
{
    private Camera cam;
    public DialogueSystem dialogueSystem;

    private bool wantsToTarget = false;

    public Transform target;
    public float visDis = 10;
    public float visAngle = 45;

    float coolDownScan = 0;
    float coolDownTarget = 0;

    private List<InteractiveItem> interactiveItems = new List<InteractiveItem>();

    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false;
        Vector3 vToThing = thing.position - transform.position;
        if (vToThing.sqrMagnitude > visDis * visDis) return false;
        if (Vector3.Angle(transform.forward, vToThing) > visAngle) return false;

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //did the user click on this game tick?
        if (cam != null && Input.GetButtonDown("Action")) wantsToTarget = true;
        if (cam != null && Input.GetButtonUp("Action")) wantsToTarget = false;

        if (!wantsToTarget) target = null;

        coolDownScan -= Time.deltaTime;
        if(coolDownScan <= 0 || (target == null)) ScanForTarget();
        coolDownTarget -= Time.deltaTime;
        if (coolDownTarget <= 0) PickATarget();
        if (target && !CanSeeThing(target)) target = null;

        RayFire();
    }

    private void ScanForTarget()
    {
        coolDownScan = 1;
        interactiveItems.Clear();
        InteractiveItem[] things = GameObject.FindObjectsOfType<InteractiveItem>();

        foreach(InteractiveItem thing in things)
        {
            if (CanSeeThing(thing.transform))
            {
                interactiveItems.Add(thing);
            }
        }
    }

    private void PickATarget()
    {
        coolDownTarget = .25f;
        target = null;
        float closetDisSoFar = 0;

        foreach(InteractiveItem pt in interactiveItems)
        {
            float dd = (pt.transform.position - transform.position).sqrMagnitude;

            if (dd < closetDisSoFar || target == null)
            {
                target = pt.transform;
                closetDisSoFar = dd;
            }
        }

    }

    private void RayFire()
    {
        //print("RAY");
        if (!wantsToTarget) return;
        if (target == null) return;
        if (!CanSeeThing(target)) return;
            //shoot a ray into the scene

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction, Color.black, 100f);
            if (Physics.Raycast(ray, out hit))
            {
                //raycast hit controller in scene

                if (DialogueSystem.inConversation == false)
                {
                    DialogueTrigger dialogueTrigger = hit.transform.GetComponent<DialogueTrigger>();
                    if (dialogueTrigger != null) dialogueTrigger.TriggerDialogue();
                }
            }
    }



}
