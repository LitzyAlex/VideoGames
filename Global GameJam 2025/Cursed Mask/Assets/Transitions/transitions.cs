using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    [Header("Animator")]
    public Animator transitionAnimator;
    public string boolSalida = "Button";
    public float tiempoTransicion = 1f; // duración de la transición

    [Header("Audio")]
    public AudioSource musicaFondo;
    public float duracionFade = 1f;

    [Header("Animación")]
    public string estadoSalida = "Salida"; // nombre EXACTO del estado
    public int animatorLayer = 0;

    private bool enTransicion = false;

    public void LoadNextScene(int sceneIndex, bool activar)
    {
        if (enTransicion) return; // 
        StartCoroutine(SceneLoad(sceneIndex, activar));
    }

    IEnumerator SceneLoad(int sceneIndex, bool activar)
    {
        Time.timeScale = 1f;

        PlayFadeOut();

        // Fade música
        if (musicaFondo != null)
        {
            float volumenInicial = musicaFondo.volume;
            float t = 0f;

            while (t < duracionFade)
            {
                t += Time.unscaledDeltaTime;
                musicaFondo.volume = Mathf.Lerp(volumenInicial, 0f, t / duracionFade);
                yield return null;
            }

            musicaFondo.volume = 0f;
        }

        yield return new WaitForSecondsRealtime(tiempoTransicion);

        SceneManager.LoadScene(sceneIndex);
    }


    public void PlayFadeOut()
    {
        transitionAnimator.Rebind();
        transitionAnimator.Update(0f);
        transitionAnimator.SetBool(boolSalida, false);
        transitionAnimator.Update(0f);
        transitionAnimator.SetBool(boolSalida, true);
    }

}



