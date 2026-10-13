using UnityEngine;

public class HumanoDash : MonoBehaviour
{
    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isDashing;
    private bool canDash = true;
    private float dashTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0)
            {
                EndDash();
            }
        }
        else
        {
            // Recargar dash al tocar el suelo
            bool isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundRadius,
                groundLayer
            );

            if (isGrounded)
            {
                canDash = true;
            }
        }
    }

    void StartDash()
    {
        canDash = false;
        isDashing = true;
        dashTime = dashDuration;
    }

    void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
