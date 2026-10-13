using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ControladorMisiones : MonoBehaviour
{
    public static ControladorMisiones Instance { get; private set; }

    [Header("Panel de instrucciones")]
    [SerializeField] private PanelInstruccionMision panelInstrucciones;

    [Header("Papelera")]
    [SerializeField] private float tiempoPapelera = 2f;

    //Impresion
    [Header("Imprimir")]
    [SerializeField] private float tiempoImpresion = 5f;
    private HashSet<TaskStation> impresionesTerminadas = new HashSet<TaskStation>();

    [Header("Excel")]
    [SerializeField] private int cantidadDigitosExcel = 4;

    [Header("Reporte")]
    [SerializeField] private int cantidadLetrasReporte = 7;

    private Coroutine misionActual;
    private TaskStation estacionActual;

    // Llevar documentos
    private bool documentosRecogidos = false;
    public bool DocumentosRecogidos => documentosRecogidos;
    private Action completarDocumentos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EjecutarMision(TipoMision tipo,TaskStation estacion,Action alCompletar)
    {
        Debug.Log("Tipo de misión recibido: " + tipo +" | Estación: " + estacion.gameObject.name);

        estacionActual = estacion;

        if (panelInstrucciones != null)
        {
            Action accionSalir;

            if (tipo == TipoMision.Imprimir)
            {
                accionSalir = () => SalirDeImprimir(estacion);
            }
            else if (tipo == TipoMision.LlevarDocumentos)
            {
                accionSalir = SalirDeLlevarDocumentos;
            }
            else
            {
                accionSalir = CancelarMision;
            }

            panelInstrucciones.Mostrar(
                tipo,
                estacion,
                accionSalir,
                () => CompletarMision(alCompletar),
                tiempoImpresion,
                cantidadDigitosExcel,
                cantidadLetrasReporte
            );
        }

        switch (tipo)
        {
            case TipoMision.Papelera:

                EjecutarPapelera(estacion, alCompletar);

                break;

            case TipoMision.Imprimir:

                EjecutarImprimir(estacion, alCompletar);

                break;

            case TipoMision.Reporte:

                EjecutarReporte(estacion, alCompletar);

                break;

            case TipoMision.Excel:

                EjecutarExcel(estacion, alCompletar);

                break;

            case TipoMision.Llamada:

                Debug.Log("Misión de llamada todavía no implementada.");

                break;

            case TipoMision.LlevarDocumentos:

                EjecutarLlevarDocumentos(estacion, alCompletar);

                break;
        }
    }

    private void EjecutarPapelera(TaskStation estacion,Action alCompletar)
    {

        misionActual = StartCoroutine(VaciarPapelera(estacion, alCompletar));
    }

    private IEnumerator VaciarPapelera(TaskStation estacion,Action alCompletar)
    {
        float tiempo = tiempoPapelera;

        while (tiempo > 0f)
        {
            if (Keyboard.current != null &&
                Keyboard.current.fKey.isPressed)
            {
                tiempo -= Time.deltaTime;
            }
            else
            {
                // Si suelta F, reiniciamos el progreso.
                tiempo = tiempoPapelera;
            }

            // Convertimos el tiempo en progreso de 0 a 1.
            float progreso = 1f - (tiempo / tiempoPapelera);

            if (panelInstrucciones != null)
            {
                panelInstrucciones.ActualizarBarraPapelera(progreso);
            }

            yield return null;
        }

        // La barra queda llena al completar la tarea.
        if (panelInstrucciones != null)
        {
            panelInstrucciones.ActualizarBarraPapelera(1f);
            panelInstrucciones.Ocultar();
        }

        misionActual = null;
        estacionActual = null;

        alCompletar?.Invoke();
    }

    private void EjecutarExcel(TaskStation estacion,Action alCompletar)
    {
        Debug.Log("Revisar datos del trimestre.");
    }

    private void EjecutarReporte(TaskStation estacion, Action alCompletar)
    {
        Debug.Log("Haciendo reporte");
    }

    private void EjecutarImprimir(TaskStation estacion, Action alCompletar)
    {
        if (impresionesTerminadas.Contains(estacion))
        {
            Debug.Log(" Puedes recoger las hojas.");

            panelInstrucciones.MostrarImpresionTerminada();

            return;
        }

        estacion.IniciarSonidoImpresora();

        misionActual = StartCoroutine(ImprimirDocumento(estacion));

    }

    private IEnumerator ImprimirDocumento(TaskStation estacion)
    {
        float tiempoRestante = tiempoImpresion;

        while (tiempoRestante > 0f)
        {
            float progreso = 1f - (tiempoRestante / tiempoImpresion);

            if (panelInstrucciones != null)
            {
                panelInstrucciones.ActualizarBarraImpresion(progreso);
            }

            tiempoRestante -= Time.deltaTime;

            yield return null;
        }

        // Apagar la impresora cuando termine el tiempo.
        estacion.DetenerSonidoImpresora();

        // La impresión terminó.
        impresionesTerminadas.Add(estacion);
        estacion.MarcarImpresionTerminada();

        if (panelInstrucciones != null)
        {
            panelInstrucciones.ActualizarBarraImpresion(1f);
            panelInstrucciones.MostrarImpresionTerminada();
        }

        estacion.PrepararParaEsperarImpresion(false);

        Debug.Log("Impresión terminada. Las hojas están listas.");

        misionActual = null;
    }

    private void SalirDeImprimir(TaskStation estacion)
    {

        estacion.PrepararParaEsperarImpresion();
    }

    //YA ANDA
    private void CompletarMision(Action alCompletar)
    {

        if (panelInstrucciones != null)
        {
            panelInstrucciones.Ocultar();
        }

        misionActual = null;
        estacionActual = null;

        alCompletar?.Invoke();
    }

    //CANCELADO
    private void CancelarMision()
    {
        Debug.Log("Misión cancelada por el jugador.");

        if (misionActual != null)
        {
            StopCoroutine(misionActual);
            misionActual = null;
        }

        if (estacionActual != null)
        {
            estacionActual.CancelarMision();
            estacionActual = null;
        }
    }

    private void EjecutarLlevarDocumentos(TaskStation estacion,Action alCompletar)
    {
        documentosRecogidos = true;
        completarDocumentos = alCompletar;

        Debug.Log("Documentos recogidos");
    }

    private void SalirDeLlevarDocumentos()
    {
        // Solo cerramos el panel.
        // NO cancelamos la misión.
        if (panelInstrucciones != null)
        {
            panelInstrucciones.Ocultar();
        }

        Debug.Log("Panel cerrado. Los documentos siguen recogidos.");
    }

    public void EntregarDocumentos()
    {
        if (!documentosRecogidos)
        {
            Debug.Log("No tienes documentos para entregar.");
            return;
        }

        documentosRecogidos = false;

        Debug.Log("Documentos entregados en Recursos Humanos.");

        completarDocumentos?.Invoke();
        completarDocumentos = null;
    }
}