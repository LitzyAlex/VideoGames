using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuSystem : MonoBehaviour
{

    private Animator transitionAnimator;
    public TransitionManager transitionManager;

    void Awake()
    {
        transitionManager = FindAnyObjectByType<TransitionManager>();

        if (transitionManager == null)
        {
            Debug.LogError("NO se encontró TransitionManager en la escena");
        }
    }


    

    public void Jugar()
    {
        transitionManager.LoadNextScene(1,true);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
