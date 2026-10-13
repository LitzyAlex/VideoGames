using UnityEngine;
using System.Collections;

public class MusicaMenu : MonoBehaviour
{
    [Header("Configuración")]
    public AudioSource audioSource;

    public float volumenFinal = 0.5f;
    public float duracionFade = 2f;

    private void Start()
    {
        // Comenzamos con el volumen en 0
        audioSource.volume = 0f;

        // Comenzamos a reproducir la música
        audioSource.Play();

        // Subimos poco a poco el volumen
        StartCoroutine(FadeInMusica());
    }

    private IEnumerator FadeInMusica()
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                0f,
                volumenFinal,
                tiempo / duracionFade
            );

            yield return null;
        }

        audioSource.volume = volumenFinal;
    }

    public void FadeOutMusica()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float tiempo = 0f;
        float volumenInicial = audioSource.volume;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                volumenInicial,
                0f,
                tiempo / duracionFade
            );

            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
