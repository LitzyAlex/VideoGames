using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Light2D globalLight;
    public Light2D luzJugador;
    public GameObject player;
    public AudioSource musica;
    public AudioSource llamada;

    [Header("Elementos a ocultar")]
    public GameObject temporizadorUI;
    public GameObject misionesUI;
    public GameObject elevador;
    public GameObject panelInstrucciones;
    public GameObject panelLlamada;

    [Header("Objetos a ocultar en Game Over")]
    public GameObject[] objetosGameOver;

    [Header("Fade de música")]
    public float duracionFade = 2f;

    private bool gameOver = false;

    [Header("Control de pausa")]
    public MonoBehaviour scriptPausa;

    void Start()
    {
        gameOverPanel.SetActive(false);
        luzJugador.enabled = false;


    }

    private IEnumerator FadeOutMusica()
    {
        llamada.Stop();
        float volumenInicial = musica.volume;

        while (musica.volume > 0)
        {
            musica.volume -= volumenInicial * Time.deltaTime / duracionFade;
            yield return null;
        }

        musica.Stop();
        
        musica.volume = volumenInicial;
    }

    public void ActivarGameOver()
    {
        
        if (gameOver)
            return;

        foreach (GameObject objeto in objetosGameOver)
        {
            objeto.SetActive(false);
        }
        gameOver = true;
        Camera.main.GetComponent<CameraFollow>().ActivarGameOver();

        temporizadorUI.SetActive(false);
        misionesUI.SetActive(false);
        elevador.SetActive(false);
        panelInstrucciones.SetActive(false);
        panelLlamada.SetActive(false);

        player.GetComponent<Player>().ActivarGameOver();

        if (musica != null)
        {
            StartCoroutine(FadeOutMusica());
        }

        // Detener al jugador
        player.GetComponent<Player>().enabled = false;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }


        // Apagar la luz global
        globalLight.intensity = 0;

        // Encender la luz del jugador
        luzJugador.intensity = 2;
        luzJugador.enabled = true;

        // Mostrar Game Over
        gameOverPanel.SetActive(true);
    }

    public void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Salir()
    {
        SceneManager.LoadScene(1);
    }
}