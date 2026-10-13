using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject CoinVidaPanel;
    public TMP_Text textPuntos;

    public TransitionManager transitionManager;
    private Animator transitionAnimator;

    void Awake()
    {
        transitionManager = FindAnyObjectByType<TransitionManager>();

        if (transitionManager == null)
        {
            Debug.LogError("NO se encontró TransitionManager en la escena");
        }
    }


    public void MostrarGameOver()
    {
        Time.timeScale = 0;
        textPuntos.text = (FindAnyObjectByType<Player>().coins).ToString();
        gameOverPanel.SetActive(true);
        CoinVidaPanel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 0;
        transitionManager.LoadNextScene(2, true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 0;
        transitionManager.LoadNextScene(1, true);
    }
}
