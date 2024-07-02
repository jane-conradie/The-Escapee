using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] AudioSource attackSFX;

    Animator animator;
    Rigidbody2D attackRigidBody2D;
    PlayerMovement player;

    float xSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackRigidBody2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;

        // set animator to firing since that is the state on instatiation
        animator.Play("Firing");
        attackSFX.Play();
    }

    void Update()
    {
        attackRigidBody2D.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // play collide animation
        animator.Play("Colliding");

        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // play collide animation
        animator.Play("Colliding");
    }

    // making public so that this can be added to the animation
    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
