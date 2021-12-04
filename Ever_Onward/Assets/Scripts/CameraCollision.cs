using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    public Camera cam;
    Vector3 cameraDirection;

    public float camDistance;
    public Vector2 camerDistanceMinMax = new Vector2(0.5f, 5f);
    public Transform cameraA;


    void Start()
    {
        cameraDirection = cameraA.position.normalized;
        camDistance = camerDistanceMinMax.y;
    }

    void Update()
    {
        print(cameraDirection + "cam direction");
        CheckCameraOcclusionAndCollision(cameraA);
    }

    public void CheckCameraOcclusionAndCollision(Transform cameraA)
    {
        Vector3 desiredCamPosition = transform.TransformPoint(cameraDirection * 3);
        RaycastHit hit;
        Debug.DrawLine(transform.position, desiredCamPosition, Color.red);
        if(Physics.Linecast(transform.position, desiredCamPosition, out hit))
        {
            camDistance = Mathf.Clamp(hit.distance * .9f, camerDistanceMinMax.x, camerDistanceMinMax.y);
        }
        else
        {
            camDistance = camerDistanceMinMax.y;
        }

            cameraA.transform.position = cameraDirection * (camDistance - .1f);

        print(cameraA.transform.position + "transform");
    }

}
