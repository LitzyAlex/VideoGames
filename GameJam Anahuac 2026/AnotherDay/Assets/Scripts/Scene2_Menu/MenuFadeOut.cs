using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuFadeOut : MonoBehaviour
{
    //Atributos
    public CanvasGroup canvasGroup;

    // Cuánto dura el Fade Out
    public float fadeOutDuration = 1f;

    public string nextSceneName;


    // Este método será llamado por el botón Jugar, por eso en public
    public void CargarJuego()
    {
        TransicionMes.mesActual = 1;
        PlayerPrefs.SetInt("UltimoNivelCompletado", 0);
        PlayerPrefs.Save();
        StartCoroutine(FadeOut());
    }


    // Hace que el menú desaparezca poco a poco
    private IEnumerator FadeOut()
    {
        float tiempo = 0f;

        while (tiempo < fadeOutDuration)
        {
            // Aumentamos el tiempo transcurrido
            tiempo += Time.deltaTime;

            canvasGroup.alpha = 1f - (tiempo / fadeOutDuration);

            yield return null;
        }


        // Nos aseguramos de terminar completamente invisibles
        canvasGroup.alpha = 0f;


        SceneManager.LoadScene(nextSceneName);
    }
}