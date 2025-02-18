using UnityEngine;

public class PlayerWallRunning : MonoBehaviour
{

    /*
     * ToDo:
     * Air mutiplier
     * Static wall jump - capacity to stick to wall, then jump from stationary. new state - wallstick / sticky?
     */

    [Header("Wall Running")]
    public LayerMask isWallRunnable;
    public LayerMask isGround;
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallJumpUpForce;
    public float wallJumpSideForce;


    [Header("Input")]
    public KeyCode wallJumpKey = KeyCode.Space;
    public KeyCode wallStickKey = KeyCode.CapsLock; // clashed with shift, needs a rework
    private float horizontalMovement;
    private float verticalMovement;

    [Header("Exiting State")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;


    [Header("Wall Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform cameraTransform;
    private PlayerMovementRB pm;
    private Rigidbody rb;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementRB>();
    }

    // Update is called once per frame
    void Update()
    {
        checkWall();
        StateManager();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
        {
            WallRunMovement();
        }
    }

    //check if the wall is wallrunnable
    //needs a rework - want only some sides to be wallrunnable, not all sides.
    private void checkWall()
    {
        wallRight = Physics.Raycast(transform.position, cameraTransform.right, out rightWallHit, wallCheckDistance, isWallRunnable);
        wallLeft = Physics.Raycast(transform.position, -cameraTransform.right, out leftWallHit, wallCheckDistance, isWallRunnable);
    }

    //check if character is airborne
    private bool isAirborne()
    {
        bool inAir = Physics.Raycast(transform.position, Vector3.down, minJumpHeight, isGround);
        return !inAir;
    }

    private void StateManager()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        //Has to be a wall, has to be pressing the W key, and above ground to start wallrun
        if (Input.GetKeyDown(wallStickKey) && isAirborne()) // trying to stick to wall...not tested sufficiently
        {
            Debug.Log("sticky activated");
            WallStick();
        }
        else if ((wallLeft || wallRight) && verticalMovement > 0f && isAirborne() && !exitingWall)
        {
            // if character is airborne, jumping onto a wall, and that wall is runnable:
            if (!pm.wallrunning)
            {
                BeginWallRun();
            }
            
            if (Input.GetKeyDown(wallJumpKey))
            {
                WallJump();
            }
            
            //bug so if user is pressing A or D but not W they are in the Air but not wall running
            //create a new state for wall sticking - allows user yto stay wheer they are and stick to the wall based on what movement key they were pressing befor (A, D).
            //from this position they can only exit after leaving the movement key, or pressing spacebar to jump somewhere else.


        }
        else if (exitingWall) //Trying to leave wall while wallrunning
        {
            if (pm.wallrunning) { StopWallRun(); }
            if (exitWallTimer > 0) { exitWallTimer -= Time.deltaTime; }
            if (exitWallTimer <= 0) { exitingWall = false; }
        }
        else
        {
            if (pm.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    private void BeginWallRun()
    {
        pm.wallrunning = true;
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        pm.sticky = false;
    }

    private void WallRunMovement()
    {
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        //has to work no matter howthe wall is rotated
        //so take the right and up direction of the wall, and return forward direction.
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //check to see which way player is facing; and ensure they run in the correct direciton.
        if ((cameraTransform.forward - wallForward).magnitude > (cameraTransform.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force); // apply the movement force

        //for outside of curved walls
        if (!(wallLeft && horizontalMovement > 0) && !(wallRight && horizontalMovement < 0)) 
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force); 
        }
    }

    // sticks player to wall if they don't want to move in any direction
    private void WallStick()
    {
        rb.useGravity = false;

        if (!pm.sticky)
        {
            pm.sticky = true;
        }
    }

    //
    private void WallJump()
    {
        //enter exit wall state
        exitingWall = true;

        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal; //normal is the vector (direction) exactly perpendicular of a flat face of geometry
        Vector3 newJumpingForce = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce; // so, apply a force perpendicular to the wall (jump off of it directly outwards)

        //reset vertical velocity and add new force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(newJumpingForce, ForceMode.Impulse);
    }

}
