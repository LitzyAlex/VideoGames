using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    

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
        gameOverPanel.SetActive(true);
       
    }

    public void Restart()
    {
        Time.timeScale = 0;
        transitionManager.LoadNextScene(3, true);
        
    }

    public void Menu()
    {
        Time.timeScale = 0;
        transitionManager.LoadNextScene(1, true);
    }
}