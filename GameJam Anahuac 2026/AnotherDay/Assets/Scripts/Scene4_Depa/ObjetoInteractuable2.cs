
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ObjetoInteractuable2 : MonoBehaviour
{
    [Header("Texto a mostrar")]
    [TextArea(2, 5)]
    [SerializeField]
    private string mensaje =
        "Presiona E nuevamente para salir.";

    [Header("Configuración")]
    [SerializeField] private string playerTag = "Player";

    [Header("Cartel de proximidad")]
    [SerializeField] private GameObject promptUI;

    [Header("Transición de escena")]
    [SerializeField] private WinFadeOut fadeOut;
    [SerializeField] private string escenaDestino = "Game";

    private bool jugadorCerca = false;
    private bool dialogoAbierto = false;
    private bool cambiandoEscena = false;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        if (!jugadorCerca || Keyboard.current == null || cambiandoEscena)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!dialogoAbierto)
            {
                AbrirDialogo();
            }
            else
            {
                SalirEscena();
            }
        }
    }

    private void AbrirDialogo()
    {
        if (DialogoUI.Instance == null)
        {
            Debug.LogWarning("No hay un DialogoUI en la escena.");
            return;
        }

        DialogoUI.Instance.Mostrar(mensaje);
        dialogoAbierto = true;

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void SalirEscena()
    {
        if (fadeOut == null)
        {
            Debug.LogWarning("No asignaste el WinFadeOut de la salida.");
            return;
        }

        cambiandoEscena = true;

        if (DialogoUI.Instance != null)
            DialogoUI.Instance.Ocultar();

        if (promptUI != null)
            promptUI.SetActive(false);

        // Elegir la escena que se cargará.
        fadeOut.nextSceneName = escenaDestino;

        // Iniciar el fundido y cargar la siguiente escena.
        fadeOut.IniciarFadeOut();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        jugadorCerca = true;

        if (!dialogoAbierto && promptUI != null)
            promptUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        jugadorCerca = false;

        if (promptUI != null)
            promptUI.SetActive(false);

        if (dialogoAbierto && DialogoUI.Instance != null)
        {
            DialogoUI.Instance.Ocultar();
            dialogoAbierto = false;
        }
    }
}

