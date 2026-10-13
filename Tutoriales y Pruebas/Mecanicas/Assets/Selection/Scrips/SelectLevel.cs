using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectLevel : MonoBehaviour
{
    public TransitionManager transitionManager;

    void Awake()
    {
        transitionManager = FindAnyObjectByType<TransitionManager>();

        if (transitionManager == null)
        {
            Debug.LogError("NO se encontró TransitionManager en la escena");
        }
    }

    public void Level1()
    {
        transitionManager.LoadNextScene(2, true);
    }

    public void Level2()
    {
        transitionManager.LoadNextScene(3, true);
    }


}
