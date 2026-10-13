using UnityEngine;
using UnityEngine.InputSystem;

public class EntregaDocumentosRH : MonoBehaviour
{
    private bool jugadorCerca = false;

    private void Update()
    {
        if (!jugadorCerca || Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (ControladorMisiones.Instance != null)
            {
                ControladorMisiones.Instance.EntregarDocumentos();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = true;

        Debug.Log("Jugador llegó a Recursos Humanos");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = false;
    }
}