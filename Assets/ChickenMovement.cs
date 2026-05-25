using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
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

    [Header("UI")]
    public TextMeshProUGUI coinsText;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;

    private bool isGrounded;

    private int jumpCount;

    private int coins = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        UpdateCoinsUI();
    }

    void Update()
    {
        // RUCH LEWO PRAWO
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
        if (Input.GetKeyDown(KeyCode.Space)
            && jumpCount < maxJumps)
        {
            Jump();
        }

        // ANIMACJA BIEGU
        if (isGrounded)
        {
            if (moveInput != 0)
            {
                anim.Play("Run");
            }
            else
            {
                anim.Play("Idle");
            }
        }

        // OBRÓT POSTACI
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
        // RESET VELOCITY Y
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0f
        );

        // SI£A SKOKU
        rb.AddForce(
            Vector2.up * jumpForce,
            ForceMode2D.Impulse
        );

        jumpCount++;

        // ANIMACJE
        if (jumpCount == 1)
        {
            anim.Play("Jump", 0, 0f);
        }
        else if (jumpCount == 2)
        {
            anim.Play("DoubleJump", 0, 0f);
        }
    }

    // ZBIERANIE COINÓW
    public void AddCoins(int amount)
    {
        coins += amount;

        UpdateCoinsUI();
    }

    void UpdateCoinsUI()
    {
        if (coinsText != null)
        {
            coinsText.text = coins + "/5";
        }
    }

    // ŒMIERÆ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle")
            || collision.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }

    // PODGL¥D GROUNDCHECKA
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