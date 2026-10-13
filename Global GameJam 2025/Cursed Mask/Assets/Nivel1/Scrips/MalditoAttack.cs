using UnityEngine;
using System.Collections;

public class MalditoAttack : MonoBehaviour
{
    public float attackRadius = 1f;
    public int attackDamage = 20;

    public float attackDuration = 0.5f; // 
    public LayerMask enemyLayer;

    private bool canAttack = true;
    private bool Attack = false;

    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;
        Attack = true;

        // activar animación
        animator.SetBool("Attack", Attack);

        // aplicar daño (una sola vez)
        DoDamage();

        // esperar a que termine la animación
        yield return new WaitForSeconds(attackDuration);

        // apagar animación
        Attack = false;
        animator.SetBool("Attack", Attack);

        canAttack = true;
    }

    void DoDamage()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(
            transform.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D enemigo in enemigos)
        {
            EnemyHealth vida = enemigo.GetComponent<EnemyHealth>();
            if (vida != null)
                vida.RecibirDanio(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    void OnDisable()
    {
        if (animator != null)
            animator.SetBool("Attack", false);
    }
}



