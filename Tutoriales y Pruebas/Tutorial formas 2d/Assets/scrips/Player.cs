using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SearchService;

public class Player : MonoBehaviour
{
    public float speed = 5; //Velocidad del jugador
    private Rigidbody2D rb2D; //Acceder al componente de fisicas

    private float move; //Movimiento

    public float jumpForce = 4; //Salto
    private bool isGrounded; //Para saber si estamos en el suelo
    public Transform groundCheck; //Para detectar si el objeto esta en el suelo
    public float groundRadius = 0.1f; //lanzar una esfera debajo del personaje para con lo que colisione detecte si es suelo o no
    public LayerMask groundLayer; //Ver si coliciono con la layer del suelo

    private Animator animator; //Acceder a la animacion

    private int coins=0;
    public TMP_Text textCoins;



    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); //Poder acceder al componente dentro de unity
        animator = GetComponent<Animator>(); 
    }


    void Update()
    {
        move = Input.GetAxisRaw("Horizontal"); //Ya viene definido en unity, utiliza las flechas o A D para moverse horizontal
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y); //Mover el componente rigidbody, vector2 es vertical y horizontal, lo ultimo es para mantener la velocidad en y

        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1); //Cambia la escala del personaje cada que se mueve 1/-1, 1, 1
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce); //Ahora se modifica el eje y, se deja el eje x
        }

        animator.SetFloat("Speed", Mathf.Abs(move)); //Mathf.Abs es para que siempre sea positivo
        animator.SetFloat("VerticalVelocity",rb2D.linearVelocity.y); //Ahora lee el vertical
        animator.SetBool("isGrounded", isGrounded);

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins++;
            textCoins.text = coins.ToString();
        }

        if (collision.transform.CompareTag("Spikes"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.transform.CompareTag("Barrel"))
        {
            Vector2 knockbackDir=(rb2D.position-(Vector2)collision.transform.position).normalized; //Se resta la posicion del personaje respecto con lo que haya colisionado con el
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(knockbackDir*3,ForceMode2D.Impulse);

            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();

            foreach (BoxCollider2D col in colliders)
            {
                col.enabled = false;
            }

            collision.GetComponent<Animator>().enabled=true;
            Destroy(collision.gameObject,0.5f);
        }

    }
}
     