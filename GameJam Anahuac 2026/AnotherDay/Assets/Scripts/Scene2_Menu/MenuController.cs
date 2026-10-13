using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //atributos
    // Nombre de la escena que contiene el juego
    public string gameSceneName;
    public GameObject optionsPanel;

    private void Start()
    {
        GameOverFinalManager.ReiniciarGameOver();
    }

    //Atributos
    public void Opciones()
    {  
        Debug.Log("Abrir opciones");
        optionsPanel.SetActive(true);
    }

    public void CerrarOpciones()
    { 
        optionsPanel.SetActive(false); 
    }

    public void Salir()
    { 
        Application.Quit(); 
        Debug.Log("Salir del juego"); // Este mensaje sirve para comprobar que salga
    }


}
