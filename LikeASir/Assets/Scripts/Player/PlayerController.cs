using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public MapHandler mapHandler;
    public CannonScript cannon;
    public bool[] martiniList;

    private Rigidbody playerRigidbody;
    public LayerMask ground;
    public LayerMask player;
    private Animator anim;
    private GameObject lastWallTouched;
    private GameObject lastWallJumped;
    private AudioSource audioSource;
    public AudioClip jumpSound;

    public float maxSlidingVelocity = 3f;
    public float maxFallingVelocity = 25f;
    public float speed;
    public float jumpForce;
    public float distToGround;
    public bool grounded = true;
    [Range(1,4)]
    public int playerNumber;
    private int idleSeconds = 3;

    //GameStuffThatIsImportant - Don't destroy players, disable them
    public int deathCount;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        mapHandler = GameObject.Find("MapHandler").GetComponent<MapHandler>();
        cannon = GetComponentInChildren<CannonScript>(true);
        martiniList = new bool[3];
        deathCount = 0;
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

	
	// Update is called once per frame
	void Update () {
        Move();
        Jump();
    }

    private void Move() {
        // Get vertical/horizontal input value with wasd or arrows
        float h = Input.GetAxisRaw("Horizontal" + playerNumber);
        if (h != 0) {
            idleSeconds = 0;
        }

        // Set the movement vector based on the axis input.
        Vector3 move = new Vector3(h, 0f, 0f);

        //Move current position.
        playerRigidbody.MovePosition(transform.position + move * speed * Time.deltaTime);

        playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxFallingVelocity);

        anim.SetInteger("Horizontal", (int)h);
    }

    private void Jump() {

        if (Input.GetButtonDown("Jump" + playerNumber) && grounded) {
            grounded = false;
            lastWallJumped = lastWallTouched;
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            PlaySound(jumpSound);

            StartCoroutine(CheckIfGrounded());
        }
    }

    IEnumerator CheckIfGrounded() {
        yield return new WaitForSeconds(0.25f);
        while (!grounded) {
            if (Physics.Raycast(transform.position, Vector3.down, distToGround, ground)) {
                grounded = true;
                lastWallJumped = lastWallTouched = null;
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            }

            yield return null;
        }
        anim.SetTrigger("LandOnGround");
    }

    public IEnumerator JumpOnPlayer() {
        RaycastHit hit;
        while (true) {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround, player)) {
                lastWallJumped = lastWallTouched = null;

                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
                playerRigidbody.AddForce(Vector3.up * jumpForce * 0.75f, ForceMode.Impulse);

                hit.rigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
                hit.rigidbody.AddForce(Vector3.down, ForceMode.Impulse);

                yield return new WaitForSeconds(0.02f);
                grounded = true;
            }
            yield return null;
        }
    }

    void IdleTimeCounter() {
        idleSeconds++;
        if(idleSeconds == 3)
            anim.SetTrigger("IdleLong");
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Wall") {
            if (other.gameObject != lastWallJumped) {
                grounded = true;
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            }
            lastWallTouched = other.gameObject;
        }  
    }

    void OnCollisionStay(Collision other) {
        if (other.gameObject.tag == "Wall" && other.gameObject != lastWallJumped) {
            playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxSlidingVelocity);
            grounded = true;
        }

    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Wall") {
            grounded = false;
            StartCoroutine(CheckIfGrounded());
        }
    }

    public void playerDied() {
        mapHandler.PlayerDied(this);
        grounded = true;
    }


    void OnDisable() {
        CancelInvoke();
        StopCoroutine(JumpOnPlayer());
    }

    void OnEnable() {
        StartCoroutine(JumpOnPlayer());
        InvokeRepeating("IdleTimeCounter", 1f, 1f);
    }

    public void PlaySound(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
