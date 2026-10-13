using UnityEngine;
using TMPro;
using System.Collections;

public class MotivacionManager : MonoBehaviour
{
    public TMP_Text textoMotivacion;

    [TextArea(2, 5)]
    public string[] frases;

    public float velocidadEscritura = 0.04f;

    private Coroutine escribiendo;

    public void MostrarMotivacion()
    {
        if (frases.Length == 0)
            return;

        string frase = frases[Random.Range(0, frases.Length)];

        if (escribiendo != null)
            StopCoroutine(escribiendo);

        escribiendo = StartCoroutine(EscribirFrase(frase));
    }

    IEnumerator EscribirFrase(string frase)
    {
        textoMotivacion.text = "";

        foreach (char letra in frase)
        {
            textoMotivacion.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }

        escribiendo = null;
    }
}