using UnityEngine;
using TMPro; //Para el texto de las monedas
using UnityEngine.SceneManagement;
using System.Collections;


public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rg2D;

    private float move; //Para el movimiento del jugador

    public float jumpForce = 4;
    private bool isGrounded;
    public Transform groundCheck; //Se crea un objeto debajo del personaje para detectar si esta en el suelo
    public float groundRadius = 0.1f;
    public LayerMask groundLayer; //Aca se guarda la layer del suelo

    private Animator animator;

    private bool recibeDanio;
    public float fuerzaRebote = 10;
    private bool invencible;


    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rg2D.linearVelocity = new Vector2(move * speed, rg2D.linearVelocity.y);

        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rg2D.linearVelocity = new Vector2(rg2D.linearVelocity.x, jumpForce);
        }




        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("VerticalVelocity", rg2D.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("recibeDanio", recibeDanio);

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer); //Lanza la esferita para saber si colisiona con la layer de floor
    }

    public void RecibeDanio(Vector2 direction, int cantDanio)
    {
        if (!recibeDanio)
        {
            if (invencible) return;

            invencible = true;

            Vector2 rebote = ((Vector2)transform.position - direction).normalized;
            rebote.y = 1;

            rg2D.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);

            animator.SetTrigger("damage");

            Invoke(nameof(DesactivaInvencible), 0.5f);
        }
    }

    public void DesactivaDanio()
    {
        recibeDanio=false;
    }

    void DesactivaInvencible()
    {
        invencible = false;
    }

}