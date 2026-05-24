using UnityEngine;
using UnityEngine.SceneManagement;

public class ChickenMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 10f;
    public int maxJumps = 2;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.35f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;

    private bool isGrounded;

    private int jumpCount;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // JEŚLI MARTWY - STOP
        if (isDead)
            return;

        // LEWO / PRAWO
        moveInput = Input.GetAxisRaw("Horizontal");

        // SPRAWDZANIE ZIEMI
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // RESET SKOKÓW
        if (isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            jumpCount = 0;

            // IDLE
            if (moveInput == 0)
            {
                anim.Play("Idle");
            }
            else
            {
                anim.Play("Run");
            }
        }

        // SKOK
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        // ANIMACJA BIEGU
        if (isGrounded && moveInput != 0)
        {
            anim.Play("Run");
        }

        // OBRÓT SPRITE
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        // RUCH
        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void Jump()
    {
        // RESET PRĘDKOŚCI Y
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0f
        );

        // SIŁA SKOKU
        rb.AddForce(
            Vector2.up * jumpForce,
            ForceMode2D.Impulse
        );

        jumpCount++;

        // ANIMACJE SKOKU
        if (jumpCount == 1)
        {
            anim.Play("Jump", 0, 0f);
        }
        else if (jumpCount == 2)
        {
            anim.Play("DoubleJump", 0, 0f);
        }
    }

    // DOTKNIĘCIE PRZESZKODY
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    // ŚMIERĆ
    void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        anim.Play("Death");

        Invoke("RestartLevel", 1f);
    }

    // RESTART SCENY
    void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // PODGLĄD GROUNDCHECKA
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}