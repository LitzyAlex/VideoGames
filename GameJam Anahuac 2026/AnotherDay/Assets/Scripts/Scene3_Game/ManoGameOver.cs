using UnityEngine;
using System.Collections;

public class ManoGameOver : MonoBehaviour
{
    [Header("Movimiento")]
    public float duracion = 1f;

    private RectTransform rectTransform;
    private Vector2 posicionFinal;

    private void Awake()
    {
        // Obtenemos el RectTransform de la mano
        rectTransform = GetComponent<RectTransform>();

        // Guardamos la posición donde queremos que termine
        posicionFinal = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        // Empezamos fuera de la pantalla, hacia la derecha
        rectTransform.anchoredPosition = new Vector2(
            posicionFinal.x + 1000f,
            posicionFinal.y
        );

        // Comenzamos el movimiento
        StartCoroutine(MoverMano());
    }

    private IEnumerator MoverMano()
    {
        float tiempo = 0f;

        Vector2 posicionInicial = rectTransform.anchoredPosition;

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;

            float progreso = tiempo / duracion;

            // Movimiento suave
            progreso = Mathf.SmoothStep(0f, 1f, progreso);

            rectTransform.anchoredPosition = Vector2.Lerp(
                posicionInicial,
                posicionFinal,
                progreso
            );

            yield return null;
        }

        // Nos aseguramos de terminar exactamente en la posición final
        rectTransform.anchoredPosition = posicionFinal;
    }
}