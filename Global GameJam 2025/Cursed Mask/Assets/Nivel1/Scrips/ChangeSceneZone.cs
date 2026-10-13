using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneZone : MonoBehaviour
{
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transitionManager.LoadNextScene(1, true);
        }
    }
}
