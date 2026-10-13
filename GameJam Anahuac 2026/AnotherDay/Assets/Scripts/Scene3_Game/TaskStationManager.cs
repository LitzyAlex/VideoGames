using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
/// <summary>
/// Manager único en la escena. GestorMisiones lo llama cada vez que activa una
/// misión nueva, pasándole el nombre y qué hacer cuando se complete.
/// Este manager elige al azar una estación libre (de cualquier piso/bloque)
/// y le muestra el "?".
/// </summary>
public class TaskStationManager : MonoBehaviour
{
    public static TaskStationManager Instance { get; private set; }

    [Header("Tiempo para reutilizar estación")]
    [SerializeField] private float tiempoReutilizacion = 4f;


    [Header("Estaciones de tarea de la escena")]
    [Tooltip("Podés arrastrarlas a mano o dejar vacío para que se busquen solas en Awake")]
    [SerializeField] private List<TaskStation> estaciones = new List<TaskStation>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (estaciones.Count == 0)
        {
            estaciones.AddRange(FindObjectsOfType<TaskStation>());
        }
    }

    /// <summary>
    /// Elige una estación libre al azar (entre todos los pisos y bloques) y le asigna la tarea.
    /// alCompletar se ejecuta una sola vez cuando el jugador la completa (ej: tildar la misión en la UI).
    /// Devuelve false si no había ninguna estación libre.
    /// </summary>
    public bool AsignarTareaAleatoria(string nombreTarea, Action alCompletar = null)
    {
        List<TaskStation> libres = estaciones.FindAll(
            e => e != null &&
                e.gameObject.activeInHierarchy &&
                !e.TieneTareaActiva &&
                !e.TieneTareaEnProceso
         );

        if (libres.Count == 0)
        {
            Debug.LogWarning($"[TaskStationManager] No hay estaciones libres para asignar: {nombreTarea}");
            return false;
        }

        TaskStation elegida = libres[UnityEngine.Random.Range(0, libres.Count)];
        elegida.ActivarTarea(nombreTarea, TipoMision.Papelera, alCompletar);
        return true;
    }


    public void NotificarEstacionLibre(TaskStation estacion)
    {
        StartCoroutine(
            EsperarParaReutilizar(estacion)
        );
    }

    private IEnumerator EsperarParaReutilizar(TaskStation estacion)
    {

        yield return new WaitForSeconds(tiempoReutilizacion);

        if (GestorMisiones.Instance != null)
        {
            GestorMisiones.Instance.EstacionDisponible(estacion);
        }
    }
}