
using UnityEngine;

public class Elevador : MonoBehaviour
{
    [SerializeField] private Transform frenteElevador;

    private Transform jugador;

    private float posicionX;
    private float posicionZ;

    private SpriteRenderer spriteJugador;
    private int ordenOriginal;

    private void Start()
    {
        // Guardamos la posición horizontal y la profundidad originales.
        posicionX = frenteElevador.position.x;
        posicionZ = frenteElevador.position.z;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugador = other.transform;

        spriteJugador = other.GetComponentInChildren<SpriteRenderer>();

        if (spriteJugador != null)
        {
            // Guardamos el orden original del personaje.
            ordenOriginal = spriteJugador.sortingOrder;

            // El personaje queda detrás del frente del elevador.
            spriteJugador.sortingOrder = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (jugador == null)
            return;

        // El frente del elevador se centra en la Y del personaje.
        frenteElevador.position = new Vector3(
            posicionX,
            jugador.position.y,
            posicionZ
        );
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Restauramos el orden original del personaje.
        if (spriteJugador != null)
        {
            spriteJugador.sortingOrder = ordenOriginal;
        }

        jugador = null;
        spriteJugador = null;
    }
}
