using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarraDeVida : MonoBehaviour
{
    public Image barraDeVida;

    public float VidaMax = 100f;
    public float VidaActual;

    public float delayMostrar = 2f;
    public float velocidadIntro = 1.5f; // velocidad de llenado inicial

    private GameOver gameOver;
    private bool muerto = false;
    private bool introTerminada = false;

    void Start()
    {
        VidaActual = VidaMax;
        barraDeVida.fillAmount = 0f; // empieza vacía

        gameOver = FindAnyObjectByType<GameOver>();

        barraDeVida.gameObject.SetActive(false);
        StartCoroutine(IntroBarra());
    }

    IEnumerator IntroBarra()
    {
        yield return new WaitForSeconds(delayMostrar);

        barraDeVida.gameObject.SetActive(true);

        float vidaTemporal = 0f;

        while (vidaTemporal < VidaMax)
        {
            vidaTemporal += velocidadIntro * Time.deltaTime * VidaMax;
            barraDeVida.fillAmount = vidaTemporal / VidaMax;
            yield return null;
        }

        barraDeVida.fillAmount = 1f;
        introTerminada = true;
    }

    void Update()
    {
        if (!introTerminada) return;

        barraDeVida.fillAmount = VidaActual / VidaMax;
    }

    public void RecibirDanio(float cantidad)
    {
        if (muerto || !introTerminada) return;

        VidaActual -= cantidad;
        VidaActual = Mathf.Clamp(VidaActual, 0, VidaMax);

        if (VidaActual <= 0)
        {
            muerto = true;
            gameOver.MostrarGameOver();
        }
    }

    public void Curar(float cantidad)
    {
        if (!introTerminada) return;

        VidaActual += cantidad;
        VidaActual = Mathf.Clamp(VidaActual, 0, VidaMax);
    }
}



