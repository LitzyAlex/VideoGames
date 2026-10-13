using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Estación donde el jugador puede iniciar una misión.
/// Al presionar E se inicia la misión, pero la estación
/// permanece ocupada hasta que la misión termina realmente.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class TaskStation : MonoBehaviour
{
    [Header("Referencias visuales")]
    [SerializeField] private GameObject signoInterrogacion;

    [SerializeField] private GameObject promptUI;

    [Header("Configuración")]
    [SerializeField] private string playerTag = "Player";

    [Header("Teléfono")]
    [SerializeField] private AudioSource sonidoTelefono;
    [SerializeField] private VibracionLlamada vibracionLlamada;

    [Header("Mensaje de llamada")]
    [SerializeField] private GameObject mensajeLlamada;

    [Header("Sonido de impresora")]
    [SerializeField] private AudioSource sonidoImpresora;

    [Header("Eventos")]
    public UnityEvent<string> OnTareaCompletada;

    public UnityEvent<float> OnProgresoTarea;

    private bool jugadorCerca = false;
    private bool tareaActiva = false;

    // Indica que la misión ya comenzó y todavía no termina.
    private bool tareaEnProceso = false;
    private bool impresionTerminada = false;
    private string tareaActual = "";
    private TipoMision tipoActual;

    private Action alCompletarCallback;

    public bool TieneTareaActiva => tareaActiva;
    public bool TieneTareaEnProceso => tareaEnProceso;
    public bool ImpresionTerminada => impresionTerminada;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;

        if (signoInterrogacion != null)
            signoInterrogacion.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        if (mensajeLlamada != null)
            mensajeLlamada.SetActive(false);
    }

    private void Update()
    {
        if (!tareaActiva || tareaEnProceso || !jugadorCerca || Keyboard.current == null)
            return;

        if (!Keyboard.current.eKey.wasPressedThisFrame)
            return;



        if (tipoActual == TipoMision.Llamada)
        {
            // Permitimos contestar la llamada mientras se realiza
            // la misión de imprimir o la de llevar documentos.
            if (GestorMisiones.Instance != null &&
                GestorMisiones.Instance.MisionEnCurso &&
                GestorMisiones.Instance.TipoMisionEnCurso != TipoMision.Imprimir &&
                GestorMisiones.Instance.TipoMisionEnCurso != TipoMision.LlevarDocumentos)
            {
                Debug.Log("Termina la misión actual antes de contestar.");
                return;
            }

            Debug.Log("Contestando llamada...");
            CompletarTarea();
            return;
        }



        // Mientras la llamada está en curso,
        // las demás misiones no pueden comenzar.
        if (GestorMisiones.Instance != null &&
            GestorMisiones.Instance.LlamadaEnCurso)
        {
            Debug.Log(" Hay una llamada en curso.");
            return;
        }

        // Si no hay llamada en curso, puede comenzar.
        CompletarTarea();
    }

    public void ActivarTarea(
        string nombreTarea,
        TipoMision tipoMision,
        Action alCompletar = null)
    {
        // Durante el Game Over no se activan nuevas llamadas.
        if (tipoMision == TipoMision.Llamada)
        {
            Debug.Log("¿Game Over activo?: " +
                GameOverFinalManager.GameOverActivo);
        }
        if (tipoMision == TipoMision.Llamada &&
            GameOverFinalManager.GameOverActivo)
        {
            return;
        }

        tareaActual = nombreTarea;
        tipoActual = tipoMision;

        tareaActiva = true;
        tareaEnProceso = false;
        impresionTerminada = false;

        alCompletarCallback = alCompletar;

        // Si esta estación es un teléfono, comienza a sonar.
        if (tipoActual == TipoMision.Llamada)
        {
            if (sonidoTelefono != null)
                sonidoTelefono.Play();

            if (vibracionLlamada != null)
                vibracionLlamada.IniciarVibracion();

            if (mensajeLlamada != null)
                mensajeLlamada.SetActive(true);
        }

        if (signoInterrogacion != null)
            signoInterrogacion.SetActive(true);

        if (jugadorCerca && promptUI != null)
            promptUI.SetActive(true);
    }

    private void CompletarTarea()
    {
        if (tareaEnProceso)
            return;

        tareaEnProceso = true;
        tareaActiva = false;

        if (GestorMisiones.Instance != null)
        {
            GestorMisiones.Instance.ComenzarMision(tipoActual);
        }

        // Si contestamos el teléfono:
        if (tipoActual == TipoMision.Llamada)
        {
            // Apagar el sonido.
            if (sonidoTelefono != null)
                sonidoTelefono.Stop();

            // Detener la vibración.
            if (vibracionLlamada != null)
                vibracionLlamada.DetenerVibracion();

            if (mensajeLlamada != null)
                mensajeLlamada.SetActive(false);

            // Ahora sí comienza oficialmente la llamada.
            if (GestorMisiones.Instance != null)
                GestorMisiones.Instance.IniciarLlamada();
        }

        if (promptUI != null)
            promptUI.SetActive(false);

        Debug.Log(
            "Iniciando misión: " + tareaActual +
            " | Estación: " + gameObject.name
        );

        OnTareaCompletada?.Invoke(tareaActual);

        if (ControladorMisiones.Instance != null)
        {
            ControladorMisiones.Instance.EjecutarMision(
                tipoActual,
                this,
                MisionTerminada
            );
        }
        else
        {
            Debug.LogError("No existe un ControladorMisiones en la escena.");
            MisionTerminada();
        }
    }

    /// <summary>
    /// Se llama SOLAMENTE cuando la misión terminó realmente.
    /// </summary>
    private void MisionTerminada()
    {
        Debug.Log(
            "Misión terminada: " + tareaActual +
            " | Estación: " + gameObject.name
        );

        if (GestorMisiones.Instance != null)
        {
            GestorMisiones.Instance.TerminarMision();
        }

        // Ahora sí desaparece el signo.
        if (signoInterrogacion != null)
            signoInterrogacion.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        // Ahora sí la estación queda libre.
        tareaEnProceso = false;
        tareaActiva = false;

        // Avisamos al GestorMisiones que la misión terminó.
        if (alCompletarCallback != null)
        {
            alCompletarCallback.Invoke();
            alCompletarCallback = null;
        }

        // Ahora sí puede recibir otra misión.
        if (TaskStationManager.Instance != null)
        {
            TaskStationManager.Instance.NotificarEstacionLibre(this);
        }

        tareaActual = "";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        jugadorCerca = true;

        if (tareaActiva && !tareaEnProceso && promptUI != null)
            promptUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        jugadorCerca = false;

        if (promptUI != null)
            promptUI.SetActive(false);
    }


    public void CancelarMision()
    {
        tareaEnProceso = false;
        tareaActiva = true;

        if (signoInterrogacion != null)
        {
            signoInterrogacion.SetActive(true);
        }

        if (jugadorCerca && promptUI != null)
        {
            promptUI.SetActive(true);
        }

    }

    //para impresion
    public void PrepararParaEsperarImpresion(bool mostrarPrompt = true)
    {
        tareaEnProceso = false;
        tareaActiva = true;

        if (signoInterrogacion != null)
            signoInterrogacion.SetActive(true);

        if (mostrarPrompt && jugadorCerca && promptUI != null)
            promptUI.SetActive(true);
    }

    public void MarcarImpresionTerminada()
    {
        impresionTerminada = true;
    }

    // Inicia el sonido de la impresora.
    public void IniciarSonidoImpresora()
    {
        if (sonidoImpresora != null && !sonidoImpresora.isPlaying)
        {
            sonidoImpresora.Play();
        }
    }

    // Detiene el sonido de la impresora.
    public void DetenerSonidoImpresora()
    {
        if (sonidoImpresora != null && sonidoImpresora.isPlaying)
        {
            sonidoImpresora.Stop();
        }
    }

    //Para llamada
    private void ApagarSonidoTelefono()
    {
        if (sonidoTelefono != null &&
            sonidoTelefono.isPlaying)
        {
            sonidoTelefono.Stop();
        }
    }

    // Apaga los efectos de llamada durante el Game Over.
    public void SilenciarLlamada()
    {
        if (sonidoTelefono != null)
            sonidoTelefono.Stop();

        if (vibracionLlamada != null)
            vibracionLlamada.DetenerVibracion();

        if (mensajeLlamada != null)
            mensajeLlamada.SetActive(false);
    }


}