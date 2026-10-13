using UnityEngine;
using System.Collections;

public class MusicaGame : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Volumen")]
    public float volumenFinal = 0.4f;
    public float duracionFade = 6f;
    public float duracionFadeOut = 1.5f;

    private void Start()
    {
        // Empezamos sin volumen
        audioSource.volume = 0f;

        // Reproducimos la música
        audioSource.Play();

        // Subimos el volumen poco a poco
        StartCoroutine(FadeInMusica());
    }

    private IEnumerator FadeInMusica()
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            audioSource.volume = Mathf.Lerp(
                0f,
                volumenFinal,
                tiempo / duracionFade
            );

            yield return null;
        }

        audioSource.volume = volumenFinal;
    }

    public void FadeOutMusica(System.Action alTerminar)
    {
        StartCoroutine(FadeOut(alTerminar));
    }

    private IEnumerator FadeOut(System.Action alTerminar)
    {
        float tiempo = 0f;
        float volumenInicial = audioSource.volume;

        while (tiempo < duracionFadeOut)
        {
            tiempo += Time.unscaledDeltaTime;

            audioSource.volume = Mathf.Lerp(
                volumenInicial,
                0f,
                tiempo / duracionFadeOut
            );

            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        // Avisar que el Fade Out terminó
        alTerminar?.Invoke();
    }
}