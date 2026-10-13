
using UnityEngine;
using UnityEngine.UI;

public class ImagenMenuFinal : MonoBehaviour
{
    [Header("Imagen del menú")]
    public Image imagenMenu;

    [Header("Sprites")]
    public Sprite imagenOriginal;
    public Sprite imagenFinal;

    void Start()
    {
        // Comprobar si se terminó el último nivel.
        bool nivelCompletado =
            PlayerPrefs.GetInt("UltimoNivelCompletado", 0) == 1;

        if (imagenMenu != null)
        {
            if (nivelCompletado)
                imagenMenu.sprite = imagenFinal;
            else
                imagenMenu.sprite = imagenOriginal;
        }
    }
}
