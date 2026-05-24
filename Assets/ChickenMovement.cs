using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;

    public Transform groundCheck;
    public float groundCheckRadius = 0.35f;
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
        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            jumpCount = 0;

            // POWRÓT DO IDLE
            anim.Play("Idle");
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        // OBRÓT
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
        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0f
        );

        rb.AddForce(
            Vector2.up * jumpForce,
            ForceMode2D.Impulse
        );

        jumpCount++;

        // ZA KA¯DYM RAZEM START OD POCZ¥TKU
        if (jumpCount == 1)
        {
            anim.Play("Jump", 0, 0f);
        }
        else if (jumpCount == 2)
        {
            anim.Play("DoubleJump", 0, 0f);
        }
    }
}