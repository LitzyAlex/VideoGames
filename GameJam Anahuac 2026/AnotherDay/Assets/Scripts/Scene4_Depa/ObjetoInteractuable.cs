using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Poné este script en un objeto vacío con Box Collider 2D (trigger) en cualquier
/// parte del mapa. Cuando el jugador entra y presiona E, se abre el cuadro de
/// diálogo con el texto configurado. Al presionar E de nuevo, o al alejarse, se cierra.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Texto a mostrar")]
    [TextArea(2, 5)]
    [SerializeField] private string mensaje = "Escribí acá el texto...";

    [Header("Configuración")]
    [SerializeField] private string playerTag = "Player";

    [Header("Cartel de proximidad (opcional)")]
    [Tooltip("Objeto que muestra algo como 'Abrir el refri', visible solo cuando el jugador está cerca")]
    [SerializeField] private GameObject promptUI;

    private bool jugadorCerca = false;
    private bool dialogoAbierto = false;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        if (promptUI != null) promptUI.SetActive(false);
    }

    private void Update()
    {
        if (!jugadorCerca || Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!dialogoAbierto) AbrirDialogo();
            else CerrarDialogo();
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
        if (promptUI != null) promptUI.SetActive(false);
    }

    private void CerrarDialogo()
    {
        if (DialogoUI.Instance != null) DialogoUI.Instance.Ocultar();
        dialogoAbierto = false;
        if (jugadorCerca && promptUI != null) promptUI.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        jugadorCerca = true;
        if (!dialogoAbierto && promptUI != null) promptUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        jugadorCerca = false;
        if (promptUI != null) promptUI.SetActive(false);
        if (dialogoAbierto) CerrarDialogo();
    }
}