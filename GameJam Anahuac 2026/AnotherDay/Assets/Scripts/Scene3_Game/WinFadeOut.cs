using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinFadeOut : MonoBehaviour
{
    [Header("Fade")]
    public CanvasGroup canvasGroup;
    public float fadeOutDuration = 1.5f;

    [Header("Siguiente escena")]
    public string nextSceneName;

    [Header("Mes")]
    public bool avanzarMes = true;

    public void IniciarFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float tiempo = 0f;

        while (tiempo < fadeOutDuration)
        {
            tiempo += Time.unscaledDeltaTime;

            canvasGroup.alpha = tiempo / fadeOutDuration;

            yield return null;
        }

        canvasGroup.alpha = 1f;

        if (avanzarMes)
        {
            TransicionMes.mesActual++;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
