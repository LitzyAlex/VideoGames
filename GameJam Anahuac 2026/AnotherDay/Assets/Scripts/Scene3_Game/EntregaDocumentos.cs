using UnityEngine;
using UnityEngine.InputSystem;

public class EntregaDocumentos : MonoBehaviour
{
    [Header("Texto para entregar")]
    [SerializeField] private GameObject promptUI;

    private bool jugadorCerca = false;

    private void Awake()
    {
        // El texto empieza oculto
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        // Si el jugador no está cerca, no hacemos nada
        if (!jugadorCerca)
            return;

        // Comprobar que tenemos el sistema de documentos
        if (ControladorMisiones.Instance == null)
            return;

        // Si ya no tiene los documentos, ocultamos el texto
        if (!ControladorMisiones.Instance.DocumentosRecogidos)
        {
            if (promptUI != null)
                promptUI.SetActive(false);

            return;
        }

        // Mostrar el mensaje
        if (promptUI != null)
            promptUI.SetActive(true);

        // Detectar E
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            ControladorMisiones.Instance.EntregarDocumentos();

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = false;

        if (promptUI != null)
            promptUI.SetActive(false);
    }
}