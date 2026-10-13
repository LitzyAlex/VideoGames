using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Atributos

    // Escala normal del boton
    private Vector3 escalaNormal;

    // Escala que tendra cuando pasemos el mouse encima
    public float escalaHover = 1.05f;

    // Velocidad de la animación
    public float velocidad = 10f;

    // Escala que queremos alcanzar
    private Vector3 escalaObjetivo;

    //Metodos

    private void Start()
    {
        // Guardamos la escala original del botón
        escalaNormal = transform.localScale;

        escalaObjetivo = escalaNormal;
    }


    // Se ejecuta automaticamente cuando el mouse esta donde el boton
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Multiplicamos la escala original por 1.05
        escalaObjetivo = escalaNormal * escalaHover;
    }


    // Se ejecuta cuando el mouse sale del boton
    public void OnPointerExit(PointerEventData eventData)
    {
        // Regresamos a la escala original
        escalaObjetivo = escalaNormal;
    }


    // Update se ejecuta cada frame
    private void Update()
    {
        // Movemos suavemente la escala actual a la objetivo. Ni mucha idea de que pasa aqui
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            escalaObjetivo,
            Time.unscaledDeltaTime * velocidad
        );
    }
}