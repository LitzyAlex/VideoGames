using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using TMPro;

public class GameOverFinalManager : MonoBehaviour
{
    [Header("Jugador y gato")]
    public GameObject player;
    public Transform gato;
    public Animator gatoAnimator;

    [Header("Elementos a ocultar")]
    public GameObject temporizadorUI;
    public GameObject misionesUI;
    public GameObject elevador;
    public GameObject panelInstrucciones;
    public GameObject panelLlamada;

    [Header("Iluminación")]
    public Light2D globalLight;
    public Light2D luzJugador;
    public float intensidadLuz = 2f;
    public float duracionCambioLuz = 1f;

    [Header("Posición final del gato")]
    public Transform posicionFinal;

    [Header("Animaciones")]
    public string animacionTriste = "GameOver";
    public string animacionFinal = "Final";
    public float esperaAntesDeFire = 3f;

    [Header("Voltear animación Fire")]
    public bool voltearFire = true;

    [Header("Texto final")]
    public GameObject panelTextoFinal;
    public TextMeshProUGUI textoFinal;

    [TextArea]
    public string segundaFrase = "Tu valor no se mide por cuánto eres capaz de soportar, sino por recordar que tú también mereces descansar.";

    public float retrasoTexto = 1f;
    public float velocidadTexto = 0.05f;
    public float esperaEntreFrases = 2f;

    [Header("Fade de salida")]
    public WinFadeOut fadeOut;
    public string escenaDestino = "Menu";
    public float esperaAntesDelFade = 3f;

    [Header("Música")]
    public AudioSource musica;
    public AudioSource llamada;
    public AudioSource efectoGameOver;
    public AudioSource musicaAnimacion;
    public AudioSource musicaFinal;



    private bool iniciado = false;
    public static bool GameOverActivo { get; private set; } = false;

    void Start()
    {
        GameOverActivo = false;

        if (textoFinal != null)
            textoFinal.gameObject.SetActive(false);

        if (luzJugador != null)
            luzJugador.enabled = false;

        panelLlamada.SetActive(false);
        llamada.Stop();
    }

    public void ActivarGameOver()
    {
        if (iniciado)
            return;

        iniciado = true;
        GameOverActivo = true;
        StartCoroutine(SecuenciaFinal());

    }

    IEnumerator SecuenciaFinal()
    {
        // Detener y ocultar cualquier llamada que esté activa.
        if (llamada != null)
            llamada.Stop();

        TaskStation[] estaciones =
            FindObjectsByType<TaskStation>(FindObjectsSortMode.None);

        foreach (TaskStation estacion in estaciones)
        {
            estacion.SilenciarLlamada();
        }

        // Detener la música del nivel
        if (musica != null)
            musica.Stop();

        // Reproducir el efecto de sonido del Game Over
        if (efectoGameOver != null)
            efectoGameOver.Play();


        if (temporizadorUI != null)
            temporizadorUI.SetActive(false);

        if (misionesUI != null)
            misionesUI.SetActive(false);

        if (elevador != null)
            elevador.SetActive(false);

        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);

        if (panelLlamada != null)
            panelLlamada.SetActive(false);

        // Activar el zoom especial de Game Over.
        CameraFollow camara = Camera.main.GetComponent<CameraFollow>();

        if (camara != null)
        {
            camara.ActivarGameOver();
        }

        // Detener al jugador.
        if (player != null)
        {
            Player controlador = player.GetComponent<Player>();

            if (controlador != null)
                controlador.enabled = false;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        // Apagar la iluminación global.
        if (globalLight != null)
            globalLight.intensity = 0f;

        // Encender el foco para iluminar al gato.
        if (luzJugador != null)
        {
            luzJugador.intensity = intensidadLuz;
            luzJugador.enabled = true;
        }

        // Primera animación: tristeza.
        if (gatoAnimator != null)
            gatoAnimator.SetBool("GameOver", true);


        // Dejar que se reproduzca la animación triste.
        yield return new WaitForSecondsRealtime(2f);

        // Apagar el foco.
        yield return StartCoroutine(
            CambiarIntensidadLuz(0f)
        );

        // Mover al gato al centro del segundo piso.
        if (gato != null && posicionFinal != null)
            gato.position = posicionFinal.position;

        // Volver a encender el foco.
        if (luzJugador != null)
            luzJugador.enabled = true;

        yield return StartCoroutine(
            CambiarIntensidadLuz(intensidadLuz)
        );





        // Segunda animación: Final.
        if (gatoAnimator != null)
        {
            gatoAnimator.ResetTrigger("Fire");
            gatoAnimator.ResetTrigger(animacionFinal);

            gatoAnimator.SetTrigger(animacionFinal);

            // Reproducir la música de esta animación.
            if (musicaAnimacion != null)
                musicaAnimacion.Play();

            // Esperar los segundos que tú configures.
            yield return new WaitForSecondsRealtime(esperaAntesDeFire);

            // Activar la siguiente animación: Fire.
            gatoAnimator.SetTrigger("Fire");
        }

        if (musicaFinal != null)
            musicaFinal.Play();





        // Esperar después de activar la animación final del gato.
        yield return new WaitForSecondsRealtime(retrasoTexto);

        // Activar el panel que contiene el texto.
        if (panelTextoFinal != null)
            panelTextoFinal.SetActive(true);

        // Mostrar las frases usando el mismo texto.
        if (textoFinal != null)
        {
            textoFinal.gameObject.SetActive(true);

            // Primera frase
            yield return StartCoroutine(MostrarFrase(textoFinal.text));

            // Esperar antes de cambiar de frase
            yield return new WaitForSecondsRealtime(esperaEntreFrases);

            // Ocultar momentáneamente el texto
            textoFinal.gameObject.SetActive(false);

            // Cambiar a la segunda frase
            textoFinal.text = segundaFrase;
            textoFinal.gameObject.SetActive(true);

            yield return StartCoroutine(MostrarFrase(segundaFrase));
        }
        else
        {
            Debug.LogWarning("No asignaste Texto Final en el Inspector.");
        }



        // Esperar antes de hacer la transición final.
        yield return new WaitForSecondsRealtime(
            esperaAntesDelFade
        );


        // Ejecutar el fade que ya existe.
        if (fadeOut != null)
        {
            PlayerPrefs.SetInt("UltimoNivelCompletado", 1);
            PlayerPrefs.Save();
            fadeOut.nextSceneName = escenaDestino;
            fadeOut.IniciarFadeOut();
        }
    }

    IEnumerator CambiarIntensidadLuz(float destino)
    {
        if (luzJugador == null)
            yield break;

        float inicio = luzJugador.intensity;
        float tiempo = 0f;

        while (tiempo < duracionCambioLuz)
        {
            tiempo += Time.unscaledDeltaTime;

            luzJugador.intensity = Mathf.Lerp(
                inicio,
                destino,
                Mathf.Clamp01(tiempo / duracionCambioLuz)
            );

            yield return null;
        }

        luzJugador.intensity = destino;
    }

    IEnumerator MostrarFrase(string frase)
    {
        textoFinal.text = frase;
        textoFinal.ForceMeshUpdate();
        textoFinal.maxVisibleCharacters = 0;

        int cantidadCaracteres = textoFinal.textInfo.characterCount;

        for (int i = 0; i < cantidadCaracteres; i++)
        {
            textoFinal.maxVisibleCharacters = i + 1;
            yield return new WaitForSecondsRealtime(velocidadTexto);
        }

        textoFinal.maxVisibleCharacters = cantidadCaracteres;
    }

    public static void ReiniciarGameOver()
    {
        GameOverActivo = false;
    }



}