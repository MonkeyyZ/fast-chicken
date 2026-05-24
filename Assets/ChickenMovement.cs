using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 8f;
    public int maxJumps = 2;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;

    private bool isGrounded;

    private int jumpCount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // LEWO / PRAWO
        moveInput = Input.GetAxisRaw("Horizontal");

        // SPRAWDZANIE ZIEMI
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // RESET SKOKÓW
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // SKOK
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        // OBRACANIE SPRITE
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // ANIMACJE
        anim.SetBool("isRunning", moveInput != 0 && isGrounded);

        anim.SetBool("isGrounded", isGrounded);

        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // RUCH
        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void Jump()
    {
        // RESET OPADANIA
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0f
        );

        // DODANIE SI£Y SKOKU
        rb.AddForce(
            Vector2.up * jumpForce,
            ForceMode2D.Impulse
        );

        jumpCount++;

        // ANIMACJE SKOKU
        if (jumpCount == 1)
        {
            anim.SetTrigger("jump");
        }
        else if (jumpCount == 2)
        {
            anim.SetTrigger("doubleJump");
        }
    }

    // RYSOWANIE GROUND CHECKA
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