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
    public float stickToWallTime = 0.5f;
    public float speed = 8f;
    public float jumpForce = 17f;
    public float distToGround = 0.85f;
    public bool grounded = true;
    [Range(1,4)]
    public int playerNumber = 1;
    public int deathCount = 0;

    int idleSeconds = 3;
    float rayCastOffsetX;
    

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

	
	// Update is called once per frame
	void Update () {
        Move();
        Jump();
    }


    // Used to control player movements
    private void Move() {
        // Get horizontal input value of corresponding playerNumber
        float h = Input.GetAxisRaw("Horizontal" + playerNumber);
        
        // If the player has moved, reset the idle timer
        if (h != 0)
            idleSeconds = 0;

        // Set the movement vector based on the player input.
        Vector3 move = new Vector3(h, 0f, 0f);
        // Move current position to target position, smoothed and scaled by speed
        playerRigidbody.MovePosition(transform.position + move * speed * Time.deltaTime);

        // Clamp player velocity with a maximum velocity.
        playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxFallingVelocity);

        // Set direction bool for animation in mechanim
        anim.SetInteger("Horizontal", (int)h);
    }


    // Used to control jumping
    private void Jump() {
        // If the jump intput is pressed and the player is grounded...
        if (Input.GetButtonDown("Jump" + playerNumber) && grounded) {
            // Set grounded to false, and save lastWallTouched in lastWallJumped (used for wallJumping) (null if not wallJumping)
            lastWallJumped = lastWallTouched;
            grounded = false;

            // Reset Y velocity and add force up corresponding to the jumpforce
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Play the jump sound
            PlaySound(jumpSound);

            // Start checking for the landing
            StartCoroutine(CheckIfGrounded());
        }
    }


    // Used to detect when the player touches the ground
    IEnumerator CheckIfGrounded() {
        // Initial delay, so the ground is not detected before jumping
        yield return new WaitForSeconds(0.25f);
        // While not grounded...
        while (!grounded) {
            // If raycast detects the ground layer at player's feet...
            if (RaycastHit(groundLayer)) {
                // Set grounded to true and empty saved walls as not wall was touched 
                grounded = true;
                lastWallJumped = lastWallTouched = null;
                // Reset velocity on landing, to cancel high velocity clipping through colliders and unbreakable momentum
                playerRigidbody.velocity = new Vector3(0f, 0f, 0f);
            }
            // Return point for coroutine
            yield return null;
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
                playerRigidbody.AddForce(Vector3.up * jumpForce * 0.75f, ForceMode.Impulse);

                // Stop ennemy player Y momemtum and add small force down.
                hit.rigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
                hit.rigidbody.AddForce(Vector3.down, ForceMode.Impulse);

                // Delay so the the player can't jump again immediately (prevent applying multiple forces at the same time)
                yield return new WaitForSeconds(0.02f);
                // Enable jumping again for the player
                grounded = true;
            }
            // Return point for coroutine
            yield return null;
        }
    }


    // Used to stick the player to the wall for easier wallJumping
    IEnumerator StickToWall() {
        // Set timer and Y position
        float stickTimer = stickToWallTime;
        float initialPosY = transform.position.y;

        // While timer not over and still grounded...
        while (stickTimer > 0 && grounded) {
            // Set Y position to initial Y position to keep the player in air
            playerRigidbody.position = new Vector3(playerRigidbody.position.x, initialPosY, playerRigidbody.position.z);
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
        if (idleSeconds == 3)
            anim.SetTrigger("IdleLong");
    }


    // Play sound corresponding to the script
    public void PlaySound(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }


    // Used to manage player's death
    public void playerDied() {
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
            // If the wall is not the lastWallJumped...
            if (other.gameObject != lastWallJumped) {
                // Enable jumping again for the player (wallJump) and start the StickToWall (easier wallJumping)
                grounded = true;
                StartCoroutine(StickToWall());
            }
            // Save the lastWallTouched
            lastWallTouched = other.gameObject;
        }  
    }


    // On ongoing collision with an other collider
    void OnCollisionStay(Collision other) {
        // If the other collider is a wall and the wall is not the lastWallJumped...
        if (other.gameObject.tag == "Wall" && other.gameObject != lastWallJumped) {
            // Slide down the wall slowly after sticking ends and set grounded again to prevent bugs
            playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxSlidingVelocity);
            grounded = true;
        }
    }


    // On exiting a collision with other collider
    void OnCollisionExit(Collision other) {
        // If the other collider is a wall...
        if (other.gameObject.tag == "Wall") {
            // Remove jumping ability (can't wall jump if not on wall)
            grounded = false;
            // Stop sticking to the wall (not on wall anymore) and start checking for when the player touches the ground
            StopCoroutine(StickToWall());
            StartCoroutine(CheckIfGrounded());
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
        grounded = true;
        // Start checking for jumping on other players and start invoking IdleTimeCounter.
        StartCoroutine(JumpOnPlayer());
        InvokeRepeating("IdleTimeCounter", 1f, 1f);
    }
}
