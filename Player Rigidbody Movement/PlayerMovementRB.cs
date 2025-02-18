using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 how do i get the move speed and wall run speed to be in the same text box?
 */

public class PlayerMovementRB : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float wallrunSpeed;
    public float maxWallRunSpeed;
    //for increasing (and decreasing) speed slowly
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;


    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier; // decreases the effects of movement once airborne.
    public bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Slope Handling")] // for sliding, probably don't need this anymore.
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Text Boxes")]
    public VelocityText wallRunText; 


    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
        wallrunning,
        sticky
    }//sticky clashes with sprinting. set to be CTRL instead?

    public bool wallrunning;
    public bool sticky;
    public bool resetSpeed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateManager();

        // handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); //can use jumpkey again after jumpCooldown has passed
        }

        //when to crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // when to stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    //manages which state the character is in
    private void StateManager()
    {
        /*if (sticky)
        {
            state = MovementState.sticky;
            rb.angularVelocity = Vector3.zero;
        }

        else*/ //don't think we should use this rn 
        // wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            //if player on a wall, and moving forwards, set desired move speed, to wallrunspeed.
            if (rb.linearVelocity.y < 0.1f)
            {
                desiredMoveSpeed = maxWallRunSpeed;
            }
            else
            {

                Debug.Log("desired: " + desiredMoveSpeed + "moveS: " + moveSpeed);
                desiredMoveSpeed = moveSpeed;
                //moveSpeed = wallrunSpeed; // multiplier?
            }
        }

        // crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // airborne
        else
        {
            state = MovementState.air;
        }

        //change the speed slowly if the difference is huge.
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {

            //smoothly transition? // has to be a new speed multiplier for us to use - max wall run speed has to be here.
            //Debug.Log("definitely starting hte coroutine......");
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
            //wallRunText.textToDisplay = $"moveSpeed: {moveSpeed}";
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope) // probably don't need this anymore
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            //limit velocity if necessary
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly transition between movespeed states.
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startSpeed = moveSpeed;
        
        while (time < difference) // need a way to exit if we want to kill the speed.
        {
            moveSpeed = Mathf.Lerp(startSpeed, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            wallRunText.textToDisplay = $"Wall Run Move Speed: {moveSpeed}";
            if (resetSpeed)
            {
                Debug.Log("reset moveSpeed");
                moveSpeed = 0;
                resetSpeed = false;
                break;
            }
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        Debug.Log("new move speed = " + moveSpeed);
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope() // deprecated code here, no longer interested in using slope mechanics for D1
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection() // deprecated code here, no longer interested in using slope mechanics for D1
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}