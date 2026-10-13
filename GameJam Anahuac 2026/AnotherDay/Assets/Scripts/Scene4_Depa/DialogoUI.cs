using UnityEngine;
using TMPro;

/// <summary>
/// Poné este script en un GameObject dentro de tu Canvas (Screen Space - Overlay),
/// junto con el Panel que hace de caja de diálogo abajo de la pantalla.
/// Solo necesitás UNO en la escena.
/// </summary>
public class DialogoUI : MonoBehaviour
{
    public static DialogoUI Instance { get; private set; }

    [Tooltip("El Panel/fondo de la caja de diálogo (hijo del Canvas)")]
    [SerializeField] private GameObject panelDialogo;

    [Tooltip("El texto TMP dentro del Panel")]
    [SerializeField] private TMP_Text textoDialogo;

    public bool EstaAbierto => panelDialogo != null && panelDialogo.activeSelf;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (panelDialogo != null) panelDialogo.SetActive(false);
    }

    public void Mostrar(string mensaje)
    {
        if (textoDialogo != null) textoDialogo.text = mensaje;
        if (panelDialogo != null) panelDialogo.SetActive(true);
    }

    public void Ocultar()
    {
        if (panelDialogo != null) panelDialogo.SetActive(false);
    }
}
