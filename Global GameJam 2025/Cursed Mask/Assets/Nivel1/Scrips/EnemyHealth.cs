using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int vida = 50;

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
