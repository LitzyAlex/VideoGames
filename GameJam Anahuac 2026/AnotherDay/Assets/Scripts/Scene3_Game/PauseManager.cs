using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    [Header("Fade de salida")]
    public WinFadeOut fadeOut;

    [Header("Música")]
    public MusicaGame musicaGame;

    public GameObject winPanel;

    private bool estaPausado = false;

    void Start()
    {
        Debug.Log("PauseManager inició en: " + SceneManager.GetActiveScene().name);

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }


    void Update()
    {
        // Comprobar si el script se está ejecutando.
        if (Keyboard.current == null)
        {
            Debug.LogWarning("No se detecta el teclado.");
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("¡Escape detectado!");

            if (estaPausado)
                Reanudar();
            else
                Pausar();
        }
    }


    public void Pausar()
    {
        estaPausado = true;
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Reanudar()
    {
        estaPausado = false;
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;

        fadeOut.nextSceneName = SceneManager.GetActiveScene().name;

        if (musicaGame != null)
        {
            musicaGame.FadeOutMusica(() =>
            {
                fadeOut.IniciarFadeOut();
            });
        }
        else
        {
            fadeOut.IniciarFadeOut();
        }
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;

        fadeOut.nextSceneName = "Menu";

        if (musicaGame != null)
        {
            musicaGame.FadeOutMusica(() =>
            {
                fadeOut.IniciarFadeOut();
            });
        }
        else
        {
            fadeOut.IniciarFadeOut();
        }
    }

    void OnEnable()
    {
        Debug.Log("PauseManager ACTIVADO", this);
    }

    void OnDisable()
    {
        Debug.LogWarning("PauseManager DESACTIVADO", this);
    }
}