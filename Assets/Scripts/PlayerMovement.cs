using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(-10f, 15f);
    [SerializeField] GameObject fire;
    [SerializeField] Transform attack;
    [SerializeField] AudioSource jumpSFX;

    Vector2 moveInput;
    Rigidbody2D playerRigidbody2D;
    Animator animator;
    CapsuleCollider2D bodyCollider2D;
    BoxCollider2D footCollider2D;
    ParticleSystem hurtParticleSystem;
    AudioSource audioSource;

    float startingGravityScale;

    bool isMovementEnabled = true;

    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        footCollider2D = GetComponent<BoxCollider2D>();
        hurtParticleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        startingGravityScale = playerRigidbody2D.gravityScale;
    }

    void Update()
    {
        if (!isMovementEnabled) { return; }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void FlipSprite()
    {
        // mathf epsilon means 0
        bool playerIsMoving = Mathf.Abs(playerRigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerIsMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody2D.velocity.x), 1f);
        }

    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody2D.velocity.y);
        playerRigidbody2D.velocity = playerVelocity;

        bool playerIsMoving = Mathf.Abs(playerRigidbody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerIsMoving);
    }

    void ClimbLadder()
    {
        // check if player is touching climbing layer
        bool isTouchingLadder = bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (!isTouchingLadder)
        {
            playerRigidbody2D.gravityScale = startingGravityScale;
            animator.SetBool("isClimbing", false);
            return;
        }

        playerRigidbody2D.gravityScale = 0f;

        Vector2 climbVelocity = new Vector2(playerRigidbody2D.velocity.x, moveInput.y * climbSpeed);
        playerRigidbody2D.velocity = climbVelocity;

        bool playerIsClimbing = Mathf.Abs(playerRigidbody2D.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerIsClimbing);
    }

    void OnMove(InputValue value)
    {
        if (!isMovementEnabled) { return; }

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isMovementEnabled) { return; }

        // check that player is touching ground layer before jump is possible
        bool isGrounded = footCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (isGrounded && value.isPressed)
        {
            // play jump SFX
            jumpSFX.Play();
            // increase y velocity of player
            playerRigidbody2D.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isMovementEnabled) { return; }

        // play attack audio
        Instantiate(fire, attack.position, transform.rotation);
    }

    void Die()
    {
        if (bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) || footCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isMovementEnabled = false;
            animator.SetTrigger("Dying");

            // fling player
            FlingPlayer();
            // add particle system
            hurtParticleSystem.Play();
            // add hurt sound
            audioSource.Play();
        }
    }

    void FlingPlayer()
    {
        playerRigidbody2D.velocity += deathKick;
    }

    // function used in animator to trigger after die animation is finished
    void ProcessPlayerDeath()
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    public void SetMoveStatus(bool movementEnabled)
    {
        isMovementEnabled = movementEnabled;
    }
}
