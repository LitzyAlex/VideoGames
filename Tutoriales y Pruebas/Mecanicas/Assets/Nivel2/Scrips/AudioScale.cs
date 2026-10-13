using UnityEngine;

public class AudioScale : MonoBehaviour
{
    public AudioSource audioSource;
    public float scaleMultiplier = 10f; //crecimiento de objeto
    public float smoothSpeed = 5f; //Suave

    Vector3 initialScale; //Guardar tamaño del objeto
    float[] samples = new float[64]; //Guardar audio

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman); //Analizar musica

        float intensity = 0f;
        for (int i = 0; i < samples.Length; i++)  //Medir volumen
        {
            intensity += samples[i];
        }

        Vector3 targetScale = initialScale + Vector3.one * intensity * scaleMultiplier; //Crece y se encoge
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );
    }
}
