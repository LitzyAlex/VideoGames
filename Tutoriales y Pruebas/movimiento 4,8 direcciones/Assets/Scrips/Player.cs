using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 5;
    private Rigidbody2D rb2D;
    private Vector2 movementImput;

    private Animator animator;

    

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        movementImput.x = Input.GetAxisRaw("Horizontal");
        movementImput.y = Input.GetAxisRaw("Vertical");

        movementImput = movementImput.normalized;

        animator.SetFloat("Horizontal", movementImput.x);
        animator.SetFloat("Vertical", movementImput.y);
        animator.SetFloat("Speed", movementImput.magnitude);
        



    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = movementImput * speed;
    }
}
