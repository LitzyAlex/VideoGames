using UnityEngine;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    public CanvasGroup fade;
    public float tiempoDesvanecer = 1.5f;

    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fade.alpha = 1;

        float tiempo = 0;

        while (tiempo < tiempoDesvanecer)
        {
            tiempo += Time.deltaTime;

            fade.alpha = Mathf.Lerp(1, 0, tiempo / tiempoDesvanecer);

            yield return null;
        }

        fade.alpha = 0;
        fade.gameObject.SetActive(false);
    }
}
