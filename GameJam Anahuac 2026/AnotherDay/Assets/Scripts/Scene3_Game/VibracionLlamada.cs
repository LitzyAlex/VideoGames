using UnityEngine;

public class VibracionLlamada : MonoBehaviour
{
    [Header("Vibración")]
    [SerializeField] private float intensidad = 0.04f;
    [SerializeField] private float rotacion = 4f;
    [SerializeField] private float velocidad = 18f;

    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;

    private bool vibrando = false;

    private void Awake()
    {
        // Guardamos la posición y rotación originales
        posicionOriginal = transform.localPosition;
        rotacionOriginal = transform.localRotation;
    }

    private void Update()
    {
        if (!vibrando)
            return;

        // Movimiento rápido de izquierda a derecha
        float movimiento =
            Mathf.Sin(Time.time * velocidad) * intensidad;

        // Pequeña inclinación del teléfono
        float giro =
            Mathf.Sin(Time.time * velocidad) * rotacion;

        transform.localPosition =
            posicionOriginal + new Vector3(movimiento, 0f, 0f);

        transform.localRotation =
            rotacionOriginal * Quaternion.Euler(0f, 0f, giro);
    }

    public void IniciarVibracion()
    {
        vibrando = true;
    }

    public void DetenerVibracion()
    {
        vibrando = false;

        // Regresamos exactamente a su posición original
        transform.localPosition = posicionOriginal;
        transform.localRotation = rotacionOriginal;
    }
}