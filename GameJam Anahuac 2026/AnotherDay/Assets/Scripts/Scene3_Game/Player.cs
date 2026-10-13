using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //atributos
    public float speed = 7;

    private Rigidbody2D rb2D;
    private Animator animator;

    private float movementInput;
    //Para la escalera
    private float verticalInput;
    private bool enEscalera = false;
    private float gravedadOriginal;



    //metodos
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravedadOriginal = rb2D.gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Escalera"))
        {
            enEscalera = true;
            rb2D.gravityScale = 0;
            speed = 3;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Escalera"))
        {
            enEscalera = false;
            rb2D.gravityScale = gravedadOriginal;
            speed = 7;

        }
    }


    void Update()
    {
        movementInput = 0;
        verticalInput = 0;


        if (Keyboard.current.aKey.isPressed)
            movementInput = -1;

        if (Keyboard.current.dKey.isPressed)
            movementInput = 1;

        if (enEscalera)
        {
            if (Keyboard.current.wKey.isPressed)
                verticalInput = 1;

            if (Keyboard.current.sKey.isPressed)
                verticalInput = -1;
        }

        animator.SetFloat("Speed", Mathf.Abs(movementInput));

        if(movementInput != 0)
        {
            transform.localScale = new Vector3(movementInput, 1, 1);
        }

    }

    public void ActivarGameOver()
    {
        animator.SetBool("GameOver", true);
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = new Vector2(movementInput * speed, verticalInput * speed);
    }

}