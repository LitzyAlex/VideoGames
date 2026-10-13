using UnityEngine;
using TMPro;
using System.Collections;

public class GameWinManager : MonoBehaviour
{
    [Header("Panel de victoria")]
    public GameObject winPanel;

    [Header("Texto de victoria")]
    public TMP_Text textoVictoria;

    [TextArea(3, 5)]
    public string textoVictoriaContenido = "¡Lo lograste!";

    public float velocidadEscritura = 0.05f;

    [Header("Referencias")]
    public Temporizador temporizador;

    [Header("Fade de salida")]
    public WinFadeOut winFadeOut;
    public float tiempoAntesDelFade = 2f;

    [Header("Música")]
    public MusicaGame musicaGame;

    private bool victoria = false;

    private void Start()
    {
        winPanel.SetActive(false);
    }

    public void ActivarVictoria()
    {
        if (victoria)
            return;

        victoria = true;

        // Detener solamente el tiempo
        temporizador.Ganar();

        // Mostrar panel
        winPanel.SetActive(true);

        // Escribir el mensaje
        StartCoroutine(EscribirTexto());
    }

    private IEnumerator EscribirTexto()
    {
        textoVictoria.text = "";

        foreach (char letra in textoVictoriaContenido)
        {
            textoVictoria.text += letra;

            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }

        // Esperamos un momento para que el jugador pueda leer el mensaje.
        yield return new WaitForSecondsRealtime(tiempoAntesDelFade);

        // Iniciamos el Fade Out.
        if (musicaGame != null)
        {
            musicaGame.FadeOutMusica(() =>
            {
                if (winFadeOut != null)
                {
                    winFadeOut.IniciarFadeOut();
                }
            });
        }
        else if (winFadeOut != null)
        {
            winFadeOut.IniciarFadeOut();
        }
    }
}