using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public GameObject pausePanel;


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

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);

    }

    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);

    }

    public void Cancel()
    {
        Time.timeScale = 0;
        transitionManager.LoadNextScene(1, true);
    }
}
