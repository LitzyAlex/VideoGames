using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideo : MonoBehaviour
{
    //atributos
    public VideoPlayer videoPlayer; // Referencia al componente Video Player que reproduce nuestro video
    public CanvasGroup canvasGroup;

    public float fadeInDuration = 1f; // Duración del Fade In en segundos
    public float fadeOutDuration = 1f;

    public string nextSceneName;

    //Metodos
    void Start()
    {
        // Comenzamos con el Canvas completamente transparente 
        // 0 = invisible  1 = completamente visible
        canvasGroup.alpha = 0f;
        // Le decimos al Video Player que ejecute el método 
        // VideoTerminado cuando el video llegue al final
        videoPlayer.loopPointReached += VideoTerminado;

        videoPlayer.Play();
        // Iniciamos la corrutina encargada del Fade In
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() //Corrutina
    {
        float tiempo = 0f;

        while (tiempo < fadeInDuration) // Repetimos mientras no haya pasado la duración que establecimos para el Fade In
        {
            tiempo += Time.deltaTime; //Suma del tiempo desde el ultimo frame
            canvasGroup.alpha = tiempo / fadeInDuration; //Calculo del Fade In
            yield return null; // Esperamos hasta el siguiente frame
        }

        canvasGroup.alpha = 1f; //Al terminar la transparencia sera visible
    }

    void VideoTerminado(VideoPlayer vp)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut() //Corrutina
    {
        float tiempo = 0f;

        while (tiempo < fadeOutDuration)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = 1f - (tiempo / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        SceneManager.LoadScene(nextSceneName); // Cambiamos a la siguiente escena
    }

}
