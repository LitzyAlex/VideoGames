using UnityEngine;
using TMPro;
using System.Collections;

public class TransicionMes : MonoBehaviour
{
    [System.Serializable]
    public class DatosMes
    {
        public string numero;

        [TextArea(2, 5)]
        public string frase;

        public string escena;
    }

    [Header("Mes que se mostrará")]
    public static int mesActual = 1;

    [Header("Textos")]
    public TMP_Text textoMes;
    public TMP_Text numeroMes;
    public TMP_Text textoFrase;

    [Header("Frases de cada mes")]
    public DatosMes[] meses;

    [Header("Tiempos")]
    public float tiempoEntreElementos = 0.5f;
    public float velocidadFrase = 0.04f;

    [Header("Fade de salida")]
    public WinFadeOut fadeOut;
    public float tiempoAntesDelFade = 2f;

    void Start()
    {
        textoMes.text = "";
        numeroMes.text = "";
        textoFrase.text = "";

        StartCoroutine(MostrarMes());
    }

    IEnumerator MostrarMes()
    {
        // Mostrar "MONTH"
        textoMes.text = "MONTH";

        yield return new WaitForSecondsRealtime(tiempoEntreElementos);

        // Comprobar que el mes existe
        if (mesActual < 1 || mesActual > meses.Length)
        {
            Debug.LogError("El número de mes no existe en la lista.");
            yield break;
        }

        // Obtener información del mes
        DatosMes datos = meses[mesActual - 1];

        // Mostrar número
        numeroMes.text = datos.numero;

        yield return new WaitForSecondsRealtime(tiempoEntreElementos);

        // Escribir frase
        yield return StartCoroutine(EscribirFrase(datos.frase));

        // Esperar antes de comenzar el fade
        yield return new WaitForSecondsRealtime(tiempoAntesDelFade);

        if (fadeOut != null)
        {
            fadeOut.nextSceneName = datos.escena;

            fadeOut.IniciarFadeOut();

        }
    }

    IEnumerator EscribirFrase(string frase)
    {
        textoFrase.text = "";

        foreach (char letra in frase)
        {
            textoFrase.text += letra;

            yield return new WaitForSecondsRealtime(velocidadFrase);
        }
    }
}