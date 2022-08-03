using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShooters : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 webswingPoint;
    public LayerMask whatIsWall;
    public FirstPerson cam;
    public Transform wrist, camera, player;
    public bool paused;
    private float maxDistance = 50f;
    private SpringJoint joint;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSwinging();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopSwinging();
        }
    }

    private void LateUpdate()
    {
        //I use late update so that the web is drawn with the player smoothly
        ShootWeb();
    }

    private void StartSwinging()
    {
        RaycastHit hit;

        //Checking if there is a wall in range where the player clicks
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsWall))
        {
            //Creating a spring joint at the point the player points to
            webswingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = webswingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, webswingPoint);

            //Joint variables. I found these values best for keeping momentum
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            joint.spring = 10f;
            joint.damper = 7f;
            joint.massScale = 1f;

            lr.positionCount = 2;

            cam.DoFov(100f);
        }
    }

    private void StopSwinging()
    {
        lr.positionCount = 0;
        Destroy(joint);
        cam.DoFov(80f);
        cam.DoTilt(0f);
    }

    private void ShootWeb()
    {
        if (!joint || paused) return;

        //Drawing the web
        lr.SetPosition(0, wrist.position);
        lr.SetPosition(1, webswingPoint);
    }
}
