using UnityEngine;
using System.Collections;

public class MenuFade : MonoBehaviour
{
    //Atributos
    public CanvasGroup canvasGroup;

    // Duración del Fade In en segundos
    public float fadeInDuration = 1f;

    //Metodos
    private void Start()
    {
        // Comenzamos completamente invisibles
        canvasGroup.alpha = 0f;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // Tiempo que ha pasado desde que comenzó el Fade
        float tiempo = 0f;

        while (tiempo < fadeInDuration)
        {
            tiempo += Time.deltaTime;

            // Convertimos el tiempo transcurrido
            // en un valor entre 0 y 1
            canvasGroup.alpha = tiempo / fadeInDuration;

            yield return null;
        }


        // Visibles again
        canvasGroup.alpha = 1f;
    }
}