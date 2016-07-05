using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public MapHandler mapHandler;
    public PlayerStats playerDisplay;
    public CannonScript cannon;
    public bool[] martiniList;
    public AudioClip jumpSound;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    Rigidbody playerRigidbody;
    Animator anim;
    GameObject lastWallTouched;
    GameObject lastWallJumped;
    AudioSource audioSource;
    RaycastHit hit;

    public float maxSlidingVelocity = 5f;
    public float maxFallingVelocity = 25f;
    public float stickToWallTime = 0.25f;
    public float speed = 8f;
    public float distToGround = 0.85f;
    [Range(1,4)]
    public int playerNumber = 0;
    [Range(1, 4)]
    public int inputNumber = 0;
    public int respawnTime = 1;

    int idleSeconds = 0;
    int idleTargetTime = 5;
    float rayCastOffsetX;
    bool checkingForGround;

    public float accelerationTimeGrounded = 0.05f;
    public float accelerationTimeAirborne = 0.1f;
    public float timeToJumpApex = 0.5f;
    public float jumpHeight = 4f;
    float jumpVelocity;
    float gravity;
    float velocityXSmoothing;
    Vector3 velocity;
    bool canJump;
    bool isAirborne;
    bool hadJumpAvailable;


    // Use this for initialization
    void Awake () {
        audioSource = GetComponent<AudioSource>();
        mapHandler = GameObject.Find("MapHandler").GetComponent<MapHandler>();
        playerDisplay = mapHandler.playerStats;
        cannon = GetComponentInChildren<CannonScript>(true);
        martiniList = new bool[3];
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rayCastOffsetX = GetComponent<BoxCollider>().bounds.size.x / 3;
    }

    void Start() {
        // Formula : deltaMovement = velocityInitial * time + (acceleration * time^2) / 2  -->  where acceleration = gravity and velocityInitial is null
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        // Formula : velocityFinal = velocityInitial + acceleration * time  -->  where velocityFinal = jumpVelocity and velocityInitial is null
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }


    // Update is called once per frame
    void Update () {
        Move();
        Jump();
    }

    void FixedUpdate() {
        // Clamp player velocity with a maximum velocity.
        playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxFallingVelocity);
        // Add manual gravity to the player
        playerRigidbody.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }


    // Used to control player movements
    private void Move() {
        // Get horizontal input value of corresponding playerNumber
        float horizontal = Input.GetAxisRaw("Horizontal" + inputNumber);
        
        // If the player has moved, reset the idle timer
        if (horizontal != 0)
            idleSeconds = 0;

        // Set the movement vector based on the player input.
        velocity.x = Mathf.SmoothDamp(velocity.x, horizontal, ref velocityXSmoothing, isAirborne ? accelerationTimeAirborne : accelerationTimeGrounded);

        // Move current position to target position, smoothed and scaled by speed
        playerRigidbody.MovePosition(transform.position + velocity * speed * Time.deltaTime);

        // Set direction bool for animation in mechanim
        anim.SetInteger("Horizontal", (int)horizontal);
    }


    // Used to control jumping
    private void Jump() {
        // If the jump intput is pressed and the player can jump...
        if (Input.GetButtonDown("Jump" + inputNumber) && canJump) {
            // Set canJump to false, and save lastWallTouched in lastWallJumped (used for wallJumping) (null if not wallJumping)
            lastWallJumped = lastWallTouched;
            canJump = false;
            isAirborne = true;

            // Reset Y velocity and add force up corresponding to the jumpVelocity
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            playerRigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);

            // Play the jump sound
            PlaySound(jumpSound);
        }
        else {
            CheckIfGrounded();
        }
    }


    // Used to detect when the player touches the ground
    void CheckIfGrounded() {
        // If raycast detects the ground layer at player's feet...
        if (RaycastHit(groundLayer)) {
            if (playerRigidbody.velocity.y <= 0) {
                canJump = true;
                isAirborne = false;
                lastWallJumped = lastWallTouched = null;
            }
        }
        else {
            isAirborne = true;
        }
    }


    // Used to detects when the player jumps on top of an other player
    IEnumerator JumpOnPlayer() {
        // Infinite looping...
        while (true) {
            // If raycast detects the player layer at player's feet...
            if (RaycastHit(playerLayer)) {
                // Empty saved walls as not wall was touched 
                lastWallJumped = lastWallTouched = null;

                // Reset Y velocity and add a small force up for jumping on the other player.
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
                playerRigidbody.AddForce(Vector3.up * jumpVelocity * 0.75f, ForceMode.Impulse);

                // Stop ennemy player Y momemtum and add small force down.
                hit.rigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
                hit.rigidbody.AddForce(Vector3.down, ForceMode.Impulse);

                // Delay so the the player can't jump again immediately (prevent applying multiple forces at the same time)
                yield return new WaitForSeconds(0.02f);
                // Enable jumping again for the player
                canJump = true;
            }
            // Return point for coroutine
            yield return null;
        }
    }


    // Used to stick the player to the wall for easier wallJumping
    IEnumerator StickToWall() {
        // Set timer and X position
        float stickTimer = stickToWallTime;
        float initialPosX = transform.position.x;

        // While timer not over and can still jump...
        while (stickTimer > 0 && canJump) {
            // Set X position to initial X position to stick the player to the wall
            playerRigidbody.position = new Vector3(initialPosX, playerRigidbody.position.y, 0);
            // Decrement the timer
            stickTimer -= Time.deltaTime;
            // Return point for coroutine
            yield return null;
        }
    }


    // Used to activate the long idle animation after no movement for a while (called every seconds with InvokeRepeating) 
    void IdleTimeCounter() {
        // Increment timer
        idleSeconds++;
        // If enough time has passed, trigger the animation
        if (idleSeconds == idleTargetTime)
            anim.SetTrigger("IdleLong");
    }


    // Play sound corresponding to the script
    public void PlaySound(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }


    // Used to manage player's death
    public void playerDied() {
        playerRigidbody.velocity = Vector3.zero;
        // Call corresponding function in mapHandler
        mapHandler.PlayerDied(this);
    }


    // Used to detect objects with corresponding layerMask under player feet
    bool RaycastHit(LayerMask layerMask) {
        // Check middle raycast
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround, layerMask))
            return true;
        // Check middle + offset right raycast
        if (Physics.Raycast(transform.position + Vector3.right * rayCastOffsetX, Vector3.down, out hit, distToGround, layerMask))
            return true;
        // Check middle + offset left raycast
        if (Physics.Raycast(transform.position + Vector3.left * rayCastOffsetX, Vector3.down, out hit, distToGround, layerMask))
            return true;

        // Nothing detected, return false
        return false;
    }


    // On entering collision with an other collider
    void OnCollisionEnter(Collision other) {
        // If the other collider is a wall...
        if (other.gameObject.tag == "Wall") {

            hadJumpAvailable = canJump;
            // If the wall is not the lastWallJumped...
            if (other.gameObject != lastWallJumped) {
                // Enable jumping again for the player (wallJump) and start the StickToWall (easier wallJumping)
                canJump = true;
                StartCoroutine(StickToWall());
            }
        }  
    }


    // On ongoing collision with an other collider
    void OnCollisionStay(Collision other) {
        // If the other collider is a wall and the wall is not the lastWallJumped...
        if (other.gameObject.tag == "Wall" && other.gameObject != lastWallJumped && playerRigidbody.velocity.y < 0) {
            // Slide down the wall slowly after sticking ends and set canJump again to prevent bugs
            playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxSlidingVelocity);
            canJump = true;
            // Save the lastWallTouched
            lastWallTouched = other.gameObject;
        }
    }


    // On exiting a collision with other collider
    void OnCollisionExit(Collision other) {
        // If the other collider is a wall...
        if (other.gameObject.tag == "Wall") {
            // Remove jumping ability (can't wall jump if not on wall)
            canJump = hadJumpAvailable;
        }
    }


    // When object is set inactive
    void OnDisable() {
        // Cancel all invoke calls and stop active coroutines
        CancelInvoke();
        StopCoroutine(JumpOnPlayer());
    }


    // When object is set active
    void OnEnable() {
        // Allow jumping
        canJump = true;
        isAirborne = false;
        checkingForGround = false;

        // Start checking for jumping on other players and start invoking IdleTimeCounter.
        StartCoroutine(JumpOnPlayer());
        InvokeRepeating("IdleTimeCounter", 1f, 1f);
    }
}
