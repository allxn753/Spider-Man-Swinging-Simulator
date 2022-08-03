using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float wallJumpUpForce;
    public float wallJumpSideForce;

    [Header("Input")]
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private RaycastHit forwardWallhit;
    private RaycastHit backWallhit;
    private bool wallLeft;
    private bool wallRight;
    private bool wallForward;
    private bool wallBack;

    [Header("References")]
    public Transform orientation;
    public FirstPerson cam;
    public WallClimbing wc;
    private PlayerMovement pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();

        if (pm.wallstick)
            WallStick();
    }

    private void CheckForWall()
    {
        //Using raycast to check if there is a wall nearby
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
        wallForward = Physics.Raycast(transform.position, orientation.forward, out forwardWallhit, wallCheckDistance, whatIsWall);
        wallBack = Physics.Raycast(transform.position, -orientation.forward, out backWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        //Seeing if the player is above the ground
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        //Getting inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            if (!pm.wallrunning)
                StartWallRun();
                StopWallStick();

            if (Input.GetKeyDown(pm.jumpKey))
                WallJump();
        }

        //Wallsticking
        else if((wallLeft || wallRight || wallBack || wallForward) && !(pm.wallrunning) && AboveGround() && !(wc.climbing))
        {
            if (!pm.wallstick)
                StartWallStick();
        }

        //Other
        else
        {
            if (pm.wallrunning)
            {
                StopWallRun();
                rb.useGravity = true;
            }

            else if ((pm.wallstick) && (wc.climbing))
            {
                StopWallStick();
                rb.useGravity = true;
            }
                
            else if (pm.wallstick)
            {
                StopWallStick();
                rb.useGravity = true;
            }
                
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        //Camera effects
        cam.DoFov(90f);

        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);

    }

    private void StartWallStick()
    {
        pm.wallstick = true;
    }

    private void WallRunningMovement()
    {
        //When wallruning we don't want to fall down
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        //The forward direction will be the cross product of the wallNormal and the upwards direciton
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        //Running straight forward
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        //Running upwards or downwards
        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
            
        //Push to wall
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    private void WallStick()
    {
        //When we stick to the wall, we are not moving, we are perched on a building
        rb.useGravity = false;
        rb.velocity = new Vector3(0f, 0f, 0f);
    }

    private void WallJump()
    {
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 force = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;

        //Reset Camera
        cam.DoFov(80f);
        cam.DoTilt(0f);
    }

    private void StopWallStick()
    {
        pm.wallstick = false;
    }
}
