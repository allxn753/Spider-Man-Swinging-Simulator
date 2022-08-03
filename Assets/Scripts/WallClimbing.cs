using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsWall;
    public FirstPerson cam;

    [Header("Climbing")]
    public float climbSpeed;
    public bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private void Update()
    {
        WallCheck();
        StateMachine();

        if (climbing) Climbing();
    }

    private void StateMachine()
    {
        //Climbing
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing) StartClimbing();
        }

        else
        {
            if (climbing) StopClimbing();
        }
    }

    private void WallCheck()
    {
        //Checking if there is a wall in front of the player
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        
        //Angle between the player's vision and the wall normal
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
    }


    private void StartClimbing()
    {
        climbing = true;
        cam.DoFov(90f);
    }

    private void Climbing()
    {
        //Changing the player velocity so that the player can move up and climb
        rb.velocity = new Vector3(rb.velocity.z, climbSpeed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        climbing = false;
        cam.DoFov(80f);
    }
    
}
