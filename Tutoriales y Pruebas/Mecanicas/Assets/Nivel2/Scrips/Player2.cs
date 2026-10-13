using UnityEngine;

public class Player2 : MonoBehaviour
{
    private Rigidbody2D rg2D;
    private Animator animator;

    public float jumpForce = 4;
    private bool isGrounded;
    public Transform groundCheck; //Se crea un objeto debajo del personaje para detectar si esta en el suelo
    public float groundRadius = 0.1f;
    public LayerMask groundLayer; //Aca se guarda la layer del suelo

    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rg2D.linearVelocity = new Vector2(rg2D.linearVelocity.x, jumpForce);
        }

        animator.SetFloat("VerticalVelocity", rg2D.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer); //Lanza la esferita para saber si colisiona con la layer de floor
    }
}
