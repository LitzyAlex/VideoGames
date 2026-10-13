using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class GestorMisiones : MonoBehaviour
{
    public static GestorMisiones Instance { get; private set; }
    //atributos para el unity
    [Header("Configuración del día")]
    public int diaActual = 1;
    public int maxMisiones;

    [Header("Misiones disponibles")]
    public List<Mision> misionesDisponibles;

    private List<Mision> misionesActivas = new List<Mision>();
    private List<Mision> misionesPendientes = new List<Mision>();
    private Dictionary<Mision, int> cantidadActivada = new Dictionary<Mision, int>();

    private int misionesActivadas = 0;
    private int misionesCompletadas = 0;
    private bool juegoTerminado = false;
    private bool llamadaEnCurso = false;
    public bool LlamadaEnCurso => llamadaEnCurso;
    private TipoMision tipoMisionEnCurso;
    public TipoMision TipoMisionEnCurso => tipoMisionEnCurso;

    [Header("Interfaz")]
    public Transform listaMisiones;
    public GameObject prefabMisionUI;

    [Header("Victoria")]
    public GameWinManager gameWinManager;

    [Header("Tiempo entre misiones")]
    public float tiempoMinimo = 20f;
    public float tiempoMaximo = 40f;

    [Header("Configuración de llamada")]
    [SerializeField] private int misionesAntesDePermitirLlamada = 3;

    private bool misionEnCurso = false;

    public bool MisionEnCurso => misionEnCurso;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        maxMisiones = 0;

        foreach (Mision mision in misionesDisponibles)
        {
            maxMisiones += mision.cantidad;
        }

        Debug.Log("Misiones totales del día: " + maxMisiones);



        StartCoroutine(ActivarMisionesAutomaticamente());
    }


    IEnumerator ActivarMisionesAutomaticamente()
    {
        while (misionesActivadas < maxMisiones && !juegoTerminado)
        {
            float tiempoEspera =
                Random.Range(tiempoMinimo, tiempoMaximo);

            yield return new WaitForSeconds(tiempoEspera);

            // Si hay llamada, esperamos hasta que termine.
            if (llamadaEnCurso)
                continue;

            ActivarMisionAleatoria();
        }
    }

    public void ActivarMisionAleatoria()
    {
        if (misionesActivadas >= maxMisiones)
        {
            Debug.Log("Ya se activaron todas las misiones del día.");
            return;
        }

        if (misionesDisponibles.Count == 0)
        {
            Debug.Log("No quedan misiones disponibles.");
            return;
        }

        // Buscar las misiones que todavía pueden aparecer
        List<Mision> misionesDisponiblesParaElegir =
            misionesDisponibles.FindAll(mision =>
            {
                if (mision.cantidad <= 0)
                    return false;

                if (!cantidadActivada.ContainsKey(mision))
                    return true;

                return cantidadActivada[mision] < mision.cantidad;
            });

        if (misionesDisponiblesParaElegir.Count == 0)
        {
            Debug.Log("Ya se alcanzó la cantidad máxima de todas las misiones.");
            return;
        }

        // Elegir CUALQUIER misión aleatoriamente
        // La llamada NO tiene prioridad para aparecer
        Mision misionElegida;

        // Durante las primeras misiones NO permitimos llamadas.
        if (misionesActivadas < misionesAntesDePermitirLlamada)
        {
            List<Mision> misionesSinLlamada =
                misionesDisponiblesParaElegir.FindAll(
                    mision => mision.tipo != TipoMision.Llamada
                );

            // Elegir solamente misiones normales
            if (misionesSinLlamada.Count > 0)
            {
                misionElegida =
                    misionesSinLlamada[
                        Random.Range(0, misionesSinLlamada.Count)
                    ];
            }
            else
            {
                Debug.LogWarning("No hay misiones normales disponibles.");
                return;
            }
        }
        else
        {
            // Después de las primeras misiones,
            // la llamada vuelve a participar normalmente en el azar.
            misionElegida =
                misionesDisponiblesParaElegir[
                    Random.Range(0, misionesDisponiblesParaElegir.Count)
                ];
        }

        if (!cantidadActivada.ContainsKey(misionElegida))
        {
            cantidadActivada[misionElegida] = 0;
        }

        cantidadActivada[misionElegida]++;

        if (misionElegida.estaciones == null ||
            misionElegida.estaciones.Count == 0)
        {
            Debug.LogWarning(
                "La misión '" + misionElegida.nombre +
                "' no tiene estaciones asignadas."
            );
            return;
        }

        misionesActivadas++;

        // Intentar colocar la misión en una estación
        IntentarAsignarMision(misionElegida);
    }

    private void IntentarAsignarMision(Mision misionElegida)
    {
        // Buscar solamente las estaciones que estén libres
        List<TaskStation> estacionesLibres =
            misionElegida.estaciones.FindAll(
                e => e != null &&
                     e.gameObject.activeInHierarchy &&
                     !e.TieneTareaActiva &&
                     !e.TieneTareaEnProceso
            );

        // No hay estaciones disponibles.
        if (estacionesLibres.Count == 0)
        {
            Debug.LogWarning(
                "No hay estaciones libres para la misión: " +
                misionElegida.nombre +
                ". Se queda pendiente."
            );

            misionesPendientes.Add(misionElegida);

            return;
        }

        // Elegir una estación libre al azar
        TaskStation estacionElegida =
            estacionesLibres[
                Random.Range(0, estacionesLibres.Count)
            ];

        // Registrar la misión activa
        misionesActivas.Add(misionElegida);

        // Crear la misión visualmente
        GameObject nuevaMision =
            Instantiate(prefabMisionUI, listaMisiones);

        MisionUI misionUI =
            nuevaMision.GetComponent<MisionUI>();

        misionUI.MostrarMision(misionElegida);

        if (misionElegida.tipo == TipoMision.Llamada)
        {
            llamadaEnCurso = true;

            Debug.Log("LLAMADA PRIORITARIA ACTIVADA");
        }

        // Activar la tarea en la estación elegida
        estacionElegida.ActivarTarea(
            misionElegida.nombre,
            misionElegida.tipo,
            () =>
            {
                misionUI.MarcarCompletada();

                // La misión ya terminó, así que deja de estar activa.
                misionesActivas.Remove(misionElegida);

                // Si era la llamada, desbloquear las demás misiones.
                if (misionElegida.tipo == TipoMision.Llamada)
                {
                    llamadaEnCurso = false;

                    Debug.Log("☎️ Llamada terminada. Misiones desbloqueadas.");
                }

                MisionCompletada();
            }
        );

        Debug.Log(
            "Nueva misión asignada: " +
            misionElegida.nombre +
            " → " +
            estacionElegida.name
        );
    }

    private void MisionCompletada()
    {
        misionesCompletadas++;

        Debug.Log("Misiones completadas: " + misionesCompletadas + "/" + maxMisiones);

        if (misionesCompletadas >= maxMisiones)
        {
            juegoTerminado = true;

            Debug.Log("¡Todas las misiones completadas!");

            if (gameWinManager != null)
            {
                gameWinManager.ActivarVictoria();
            }
        }
    }

    public void EstacionDisponible(TaskStation estacion)
    {
        if (misionesPendientes.Count == 0)
            return;

        for (int i = 0; i < misionesPendientes.Count; i++)
        {
            Mision misionPendiente = misionesPendientes[i];

            // Comprobar si esta estación puede realizar la misión
            if (misionPendiente.estaciones.Contains(estacion))
            {
                misionesPendientes.RemoveAt(i);

                Debug.Log(
                    "Estación disponible: " +
                    estacion.name +
                    ". Asignando misión pendiente: " +
                    misionPendiente.nombre
                );

                IntentarAsignarMision(misionPendiente);

                return;
            }
        }

        Debug.Log(
            "La estación " + estacion.name +
            " está disponible, pero ninguna misión pendiente puede usarla."
        );
    }

    public bool HayMisionEnCurso()
    {
        return misionesActivas.Count > 0;
    }

    public void IniciarLlamada()
    {
        llamadaEnCurso = true;

        Debug.Log(" Llamada iniciada. Las demás misiones están bloqueadas.");
    }

    public void ComenzarMision(TipoMision tipo)
    {
        misionEnCurso = true;
        tipoMisionEnCurso = tipo;
    }

    public void TerminarMision()
    {
        misionEnCurso = false;
    }


}