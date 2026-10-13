using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform humano;
    public Transform maldito;

    public float detectionRadius = 3f;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;

    public float damage = 10f;
    public float damageCooldown = 1f;

    private bool canDamage = true;
    public float knockbackForce = 6f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Transform target = null;

        if (humano.gameObject.activeInHierarchy)
            target = humano;
        else if (maldito.gameObject.activeInHierarchy)
            target = maldito;

        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= detectionRadius)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);

            // 👇 VOLTEAR ENEMIGO
            if (movement.x != 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Sign(movement.x),
                    1,
                    1
                );
            }
        }
        else
        {
            movement = Vector2.zero;
        }
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canDamage) return;

        if (collision.transform.CompareTag("Player"))
        {
            BarraDeVida vida =
                collision.transform.GetComponentInParent<BarraDeVida>();

            Rigidbody2D playerRb =
                collision.transform.GetComponent<Rigidbody2D>();

            if (vida != null)
            {
                vida.RecibirDanio(damage);

                if (playerRb != null)
                {
                    Vector2 direction =
                        (collision.transform.position - transform.position).normalized;

                    playerRb.AddForce(
                        new Vector2(direction.x, 1f) * knockbackForce,
                        ForceMode2D.Impulse
                    );
                }

                StartCoroutine(Cooldown());
            }
        }
    }


    System.Collections.IEnumerator Cooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}

