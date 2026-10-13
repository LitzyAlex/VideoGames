using UnityEngine;
using TMPro;

public class Temporizador : MonoBehaviour
{
    public float tiempoInicial = 480f; // 8 minutos
    private float tiempoActual;
    private bool juegoTerminado = false; //para ganador

    public TMP_Text textoTiempo;

    public GameOverManager gameOverManager;
    public GameOverFinalManager gameOverFinalManager;

    void Start()
    {
        tiempoActual = tiempoInicial;
    }

    void Update()
    {
        if (juegoTerminado)
            return;

        if (tiempoActual > 0)
        {
            tiempoActual -= Time.deltaTime;

            if (tiempoActual <= 0)
            {
                tiempoActual = 0;
                ActualizarTexto();

                // Activar Game Over
                if (gameOverFinalManager != null)
                {
                    gameOverFinalManager.ActivarGameOver();
                }
                else if (gameOverManager != null)
                {
                    gameOverManager.ActivarGameOver();
                }

                return;
            }

            ActualizarTexto();
        }
    }

    public void Ganar()
    {
        if (juegoTerminado)
            return;

        juegoTerminado = true;

        // Detener el tiempo
    }

    void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoActual / 60);
        int segundos = Mathf.FloorToInt(tiempoActual % 60);

        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
