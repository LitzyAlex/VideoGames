using UnityEngine;
using TMPro;

public class MisionUI : MonoBehaviour
{
    public TMP_Text textoMision;

    private Mision misionAsociada;

    public void MostrarMision(Mision mision)
    {
        misionAsociada = mision;
        textoMision.text = "□ " + mision.nombre;
    }

    /// <summary>Se llama desde TaskStation cuando el jugador completa la tarea en el mapa.</summary>
    public void MarcarCompletada()
    {
        if (misionAsociada != null)
            textoMision.text = "☑ " + misionAsociada.nombre;

        textoMision.fontStyle = FontStyles.Strikethrough;

        Destroy(gameObject, 0.5f);  //destruir para que no se vea
    }
}