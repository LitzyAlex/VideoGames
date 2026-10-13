using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CartaIntro : MonoBehaviour
{
    public CanvasGroup carta;
    public Player player;

    public float tiempoVisible = 2f;
    public float tiempoDesvanecer = 1.5f;

    void Start()
    {
        Time.timeScale = 1f;

        player.enabled = false;

        StartCoroutine(MostrarCarta());
    }

    IEnumerator MostrarCarta()
    {
        carta.alpha = 1;

        yield return new WaitForSeconds(tiempoVisible);

        float tiempo = 0;

        while (tiempo < tiempoDesvanecer)
        {
            tiempo += Time.deltaTime;

            carta.alpha = Mathf.Lerp(1, 0, tiempo / tiempoDesvanecer);

            yield return null;
        }

        carta.gameObject.SetActive(false);

        player.enabled = true;
    }
}