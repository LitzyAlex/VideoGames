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

    public int coins = 0;
    public TMP_Text textCoins;

    public AudioSource audioSource;
    public AudioClip coinClip;
    public AudioClip barrelClip;
    public AudioClip vidaClip;

    private int vidas = 3;
    public Vidas vidasUI;
    private bool Muerto=false;



    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rg2D.linearVelocity = new Vector2(move*speed,rg2D.linearVelocity.y);

        if (move != 0 && Muerto==false)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);
        }

        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            rg2D.linearVelocity = new Vector2(rg2D.linearVelocity.x,jumpForce);
        }

        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("VerticalVelocity", rg2D.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
        

    }

    private void FixedUpdate()
    {
        isGrounded=Physics2D.OverlapCircle(groundCheck.position,groundRadius,groundLayer); //Lanza la esferita para saber si colisiona con la layer de floor
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins++;
            textCoins.text = coins.ToString();
            audioSource.PlayOneShot(coinClip);

        }


        //Vidas luego
        if (collision.transform.CompareTag("Spikes"))
        {
            PerderVidas();

        }

        if (collision.transform.CompareTag("Vacio"))
        {
            vidas = 1;
            PerderVidas();

        }

        if (collision.transform.CompareTag("Corazon"))
        {
            RecuperarVidas();
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(vidaClip);

        }



        if (collision.transform.CompareTag("Barrel"))
        {
            Vector2 knockbackDir=(rg2D.position-(Vector2)collision.transform.position).normalized; //Saber la direccion para saber donde mandar al personaje
            rg2D.linearVelocity = Vector2.zero;
            rg2D.AddForce(knockbackDir * 3, ForceMode2D.Impulse);

            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();

            foreach (BoxCollider2D col in colliders)
            {
                col.enabled = false;
            }

            collision.GetComponent<Animator>().enabled=true;
            Destroy(collision.gameObject,0.5f);
            audioSource.PlayOneShot(barrelClip);

        }

        if (collision.transform.CompareTag("Meta"))
        {
            FindAnyObjectByType<Win>().MostrarWin();

        }

    }

    public void PerderVidas()
    {
        if (Muerto==false)
        {
            vidas--;
            vidasUI.DesactivarVidas(vidas);
        }

        if (vidas <= 0)
        {
            Muerto = true;
            StartCoroutine(MuerteCoroutine());
        }
    }

    IEnumerator MuerteCoroutine()
    {
        animator.SetTrigger("Vida");

        rg2D.linearVelocity = Vector2.zero;
        
        rg2D.simulated = false; // bloquea movimiento y fisicas

        yield return new WaitForSeconds(1f); // tiempo para la animación

        FindAnyObjectByType<GameOver>().MostrarGameOver();
    }


    public void RecuperarVidas()
    {
        if (vidas < 3)
        {
            vidasUI.ActivarVida(vidas);
        }
        vidas++;
        

    }


}
