using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Win : MonoBehaviour
{
    public GameObject WinPanel;
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


    public void MostrarWin()
    {
        Time.timeScale = 0;
        textPuntos.text = (FindAnyObjectByType<Player>().coins).ToString();
        WinPanel.SetActive(true);
        CoinVidaPanel.SetActive(false);
    }


    public void Menu()
    {
        transitionManager.LoadNextScene(1,true);
    }

}
