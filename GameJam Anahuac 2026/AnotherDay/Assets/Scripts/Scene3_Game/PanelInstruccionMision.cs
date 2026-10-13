using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections;


public class PanelInstruccionMision : MonoBehaviour
{
    [SerializeField] private TMP_Text titulo;
    [SerializeField] private TMP_Text instruccion;
    [SerializeField] private Button botonSalir;

    [Header("Revisar datos, para el Excel")]
    [SerializeField] private Transform contenedorNumeros;
    [SerializeField] private GameObject prefabNumero;

    [Header("Configuración de llamada")]
    [SerializeField] private float tiempoLlamada = 5f;

    [Header("Barra de progreso")]
    [SerializeField] private GameObject barraProgreso;
    [SerializeField] private UnityEngine.UI.Slider progresoBarra;

    private Coroutine rutinaLlamada;

    private Player player;
    private Rigidbody2D rb;

    private Action alSalir;

    //Datos para el Excel
    private Action alCompletar;

    // Datos de la misión Reporte
    private string secuenciaReporte;
    private int posicionReporte;
    private TipoMision tipoActual;
    private TaskStation estacionActual;
    private bool mostrandoImpresion = false;
    private bool impresionFinalizada = false;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (barraProgreso != null)
            barraProgreso.SetActive(false);

        player = FindFirstObjectByType<Player>();

        if (contenedorNumeros != null)
        {
            contenedorNumeros.gameObject.SetActive(false);
        }

        if (player != null)
        {
            rb = player.GetComponent<Rigidbody2D>();
        }

        if (botonSalir != null)
        {
            botonSalir.onClick.AddListener(Salir);
        }
    }

    public void Mostrar(TipoMision tipo, TaskStation estacion,Action alSalir = null,Action alCompletar = null, float tiempoImpresion = 0f, int cantidadDigitosExcel = 4, int cantidadLetrasReporte = 7)
    {
        tipoActual = tipo;
        estacionActual = estacion;
        mostrandoImpresion = tipo == TipoMision.Imprimir;
        this.alSalir = alSalir;
        this.alCompletar = alCompletar;

        // Ocultamos la barra al abrir cualquier misión.
        if (barraProgreso != null)
            barraProgreso.SetActive(false);

        switch (tipo)
        {
            case TipoMision.Papelera:
                titulo.text = "Vaciar papelera";
                instruccion.text =
                    "Mantén presionada la tecla F para vaciar la papelera.";

                if (barraProgreso != null)
                    barraProgreso.SetActive(true);

                if (progresoBarra != null)
                    progresoBarra.value = 0f;

                break;

            case TipoMision.Imprimir:
                titulo.text = "Imprimir documento";
                instruccion.text =
                    "El documento se está imprimiendo...\n\n" +
                    "Puedes continuar con otras tareas.";

                if (barraProgreso != null)
                    barraProgreso.SetActive(true);

                if (progresoBarra != null)
                    progresoBarra.value = 0f;

                break;

            case TipoMision.Reporte:
                titulo.text = "Hacer reporte";

                if (barraProgreso != null)
                    barraProgreso.SetActive(true);

                if (progresoBarra != null)
                    progresoBarra.value = 0f;

                PrepararReporte(cantidadLetrasReporte);
                ActualizarProgresoReporte();

                break;

            case TipoMision.Excel:
                titulo.text = "Revisar datos del trimestre";
                instruccion.text =
                    "Encuentra el dato que es diferente.";
                break;

            case TipoMision.Llamada:
                titulo.text = "Realizar llamada";

                instruccion.text =
                    "Hola, podrias atenderme porfavor? " +
                    "necesito que anotes unas cosas";

                if (barraProgreso != null)
                    barraProgreso.SetActive(true);

                if (progresoBarra != null)
                    progresoBarra.value = 0f;

                break;

            case TipoMision.LlevarDocumentos:
                titulo.text = "Llevar documentos";
                instruccion.text =
                    "Documentos recogidos. Llévalos a Recursos Humanos.\n\n" +
                    "Es urgente. NO se recomienda hacer más tareas hasta llevar los documento";
                break;
        }

        // Mostrar los números solamente para la misión de Excel
        if (tipo == TipoMision.Excel)
        {
            MostrarDatos(cantidadDigitosExcel);
        }
        else
        {
            if (contenedorNumeros != null)
            {
                contenedorNumeros.gameObject.SetActive(false);
            }
        }

        // Detenemos al personaje al abrir el panel
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (player != null)
        {
            Animator animator = player.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }

            player.enabled = false;
        }

        gameObject.SetActive(true);

        // La corrutina debe comenzar DESPUÉS de activar el panel.
        if (tipo == TipoMision.Llamada)
        {
            if (rutinaLlamada != null)
                StopCoroutine(rutinaLlamada);

            rutinaLlamada = StartCoroutine(EsperarLlamada());
        }

        if (botonSalir != null)
        {
            botonSalir.interactable = tipo != TipoMision.Llamada;
        }
    }

    // Se usa cuando la misión termina correctamente
    public void Ocultar()
    {
        gameObject.SetActive(false);

        if (player != null)
        {
            player.enabled = true;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Se usa cuando el jugador presiona SALIR
    private void Salir()
    {
        Ocultar();

        Action callback = alSalir;
        alSalir = null;

        callback?.Invoke();
    }

    //Excel
    private void MostrarDatos(int cantidadDigitos)
    {
        if (contenedorNumeros == null || prefabNumero == null)
        {
            Debug.LogWarning(
                "Falta asignar ContenedorNumeros o PrefabNumero en PanelInstruccionMision."
            );

            return;
        }

        contenedorNumeros.gameObject.SetActive(true);

        // Borrar números anteriores
        foreach (Transform hijo in contenedorNumeros)
        {
            Destroy(hijo.gameObject);
        }

        // Generar un número base de 4 dígitos
        int minimo = (int)Mathf.Pow(10, cantidadDigitos - 1);
        int maximo = (int)Mathf.Pow(10, cantidadDigitos);

        int numeroCorrecto = UnityEngine.Random.Range(minimo, maximo);

        // Copiamos el número para crear el diferente
        int numeroDiferente = numeroCorrecto;

        // Elegimos qué dígito vamos a cambiar
        int posicionDigito = UnityEngine.Random.Range(0, cantidadDigitos);

        // Convertimos el número a texto para poder cambiar un dígito
        char[] digitos = numeroDiferente.ToString().ToCharArray();

        char digitoOriginal = digitos[posicionDigito];
        char nuevoDigito;

        // Asegurarnos de que el nuevo dígito sea diferente
        do
        {
            nuevoDigito = (char)('0' + UnityEngine.Random.Range(0, 10));
        }
        while (nuevoDigito == digitoOriginal);

        digitos[posicionDigito] = nuevoDigito;

        numeroDiferente = int.Parse(new string(digitos));

        // Elegimos aleatoriamente dónde estará el diferente
        int posicionDiferente = UnityEngine.Random.Range(0, 12);

        for (int i = 0; i < 12; i++)
        {
            GameObject nuevoNumero =
                Instantiate(prefabNumero, contenedorNumeros);

            Button boton =
                nuevoNumero.GetComponent<Button>();

            TMP_Text texto =
                nuevoNumero.GetComponentInChildren<TMP_Text>();

            bool esDiferente = i == posicionDiferente;

            if (esDiferente)
            {
                texto.text = numeroDiferente.ToString();
            }
            else
            {
                texto.text = numeroCorrecto.ToString();
            }

            boton.onClick.AddListener(() =>
            {
                if (esDiferente)
                {
                    Debug.Log("¡Encontraste el dato diferente!");

                    alCompletar?.Invoke();
                }
            });
        }
    }

    //Reporte
    private void PrepararReporte(int cantidadLetras)
    {
        string letrasDisponibles = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        secuenciaReporte = "";

        posicionReporte = 0;

        for (int i = 0; i < cantidadLetras; i++)
        {
            int indice = UnityEngine.Random.Range(0, letrasDisponibles.Length);

            secuenciaReporte += letrasDisponibles[indice];
        }

        posicionReporte = 0;
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
            return;

        if (tipoActual == TipoMision.Reporte)
        {
            if (secuenciaReporte == null || secuenciaReporte.Length == 0)
                return;

            DetectarTeclaReporte();
        }

        if (tipoActual == TipoMision.Imprimir)
        {
            DetectarTeclaImprimir();
        }
    }

    private void DetectarTeclaReporte()
    {
        if (Keyboard.current == null)
            return;

        foreach (KeyControl tecla in Keyboard.current.allKeys)
        {
            if (!tecla.wasPressedThisFrame)
                continue;

            string nombreTecla = tecla.displayName.ToUpper();

            if (nombreTecla.Length != 1)
                continue;

            char letraPresionada = nombreTecla[0];

            char letraEsperada = secuenciaReporte[posicionReporte];

            if (letraPresionada == letraEsperada)
            {
                posicionReporte++;
                ActualizarProgresoReporte();

                if (posicionReporte >= secuenciaReporte.Length)
                {
                    //ya esta el reporte
                    secuenciaReporte = null;

                    alCompletar?.Invoke();
                }
            }

            break;
        }
    }

    private string ObtenerSecuenciaVisual()
    {
        string resultado = "";

        for (int i = 0; i < secuenciaReporte.Length; i++)
        {
            if (i < posicionReporte)
            {
                resultado += "<color=green>" + secuenciaReporte[i] + "</color> ";
            }
            else
            {
                resultado += secuenciaReporte[i] + " ";
            }
        }

        return resultado;
    }

    private void ActualizarProgresoReporte()
    {
        instruccion.text =
            "Presiona las siguientes letras en orden:\n\n" +
            ObtenerSecuenciaVisual();

        float progreso = (float)posicionReporte / secuenciaReporte.Length;

        ActualizarBarraReporte(progreso);
    }

    //Impresion

    public void MostrarImpresionTerminada()
    {
        if (!mostrandoImpresion)
            return;

        titulo.text = "Impresión terminada";
        instruccion.text =
            "El documento ya está listo.\n\nPresiona P para recoger las hojas.";

        // Ocultamos la barra mientras el jugador recoge las hojas.
        if (barraProgreso != null)
            barraProgreso.SetActive(false);
    }


    public void ActualizarTiempoImpresion(float tiempoRestante)
    {
        // Si ya estamos mostrando otra misión,
        // no cambiamos su texto.
        if (!mostrandoImpresion)
            return;

        instruccion.text =
            "El documento se está imprimiendo...\n\n" +
            "Tiempo restante: " + Mathf.Ceil(tiempoRestante) + " segundos.\n\n" +
            "Puedes continuar con otras tareas.";
    }

    public void ActualizarTiempoPapelera(float tiempoRestante)
    {
        instruccion.text =
            "Mantén presionada la tecla F para vaciar la papelera.\n\n" +
            "Tiempo restante: " + Mathf.Ceil(tiempoRestante) + " segundos.";
    }

    private void DetectarTeclaImprimir()
    {
        if (Keyboard.current == null)
            return;

        // La P solamente funciona si ESTA impresora ya terminó.
        if (estacionActual == null || !estacionActual.ImpresionTerminada)
            return;

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("Hojas recogidas.");

            alCompletar?.Invoke();
        }
    }

    //Llamada
    private IEnumerator EsperarLlamada()
    {
        float tiempoRestante = tiempoLlamada;

        while (tiempoRestante > 0f)
        {
            // Calcular progreso de 0 a 1
            float progreso = 1f - (tiempoRestante / tiempoLlamada);

            // Actualizar la barra
            ActualizarBarraLlamada(progreso);

            instruccion.text =
                "Hola, podrias atenderme porfavor? " +
                "necesito que anotes unas cosas";


            tiempoRestante -= Time.deltaTime;

            yield return null;
        }

        // La barra queda completamente llena
        ActualizarBarraLlamada(1f);

        instruccion.text =
            "Llamada terminada.";

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Completando llamada.");

        alCompletar?.Invoke();

        rutinaLlamada = null;
    }

    // Actualiza el progreso de la papelera.
    public void ActualizarBarraPapelera(float progreso)
    {
        if (tipoActual != TipoMision.Papelera)
            return;

        if (progresoBarra != null)
            progresoBarra.value = Mathf.Clamp01(progreso);
    }

    // Actualiza el progreso de la impresión.
    public void ActualizarBarraImpresion(float progreso)
    {
        if (tipoActual != TipoMision.Imprimir)
            return;

        if (progresoBarra != null)
            progresoBarra.value = Mathf.Clamp01(progreso);
    }

    // Actualiza el progreso de la llamada.
    public void ActualizarBarraLlamada(float progreso)
    {
        if (tipoActual != TipoMision.Llamada)
            return;

        if (progresoBarra != null)
            progresoBarra.value = Mathf.Clamp01(progreso);
    }

    public void ActualizarBarraReporte(float progreso)
    {
        if (tipoActual != TipoMision.Reporte)
            return;

        if (progresoBarra != null)
            progresoBarra.value = Mathf.Clamp01(progreso);
    }
}