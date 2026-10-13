using UnityEngine;
using System.Collections;

public class AudioBeatTilemap : MonoBehaviour
{
    public AudioSource audioSource;

    
    public float verticalAmount = 1.5f; //cantidad de subida y bajada
    public float verticalSmooth = 6f;  //suavidad

    
    public float horizontalSpeed = 3f;
    public float startDelay = 3f; //Espera

    
    public int bassSamples = 8; //beat
    public float beatThreshold = 0.03f;  //fuerza
    public float beatCooldown = 0.3f;

    Rigidbody2D rb;

    float baseY; //Posicion original
    float lowY;
    float lastBeatTime;
    float prevBass;

    bool isUp = true; 
    bool moveHorizontally = false;

    float[] samples = new float[64];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Guarda posiciones
        baseY = rb.position.y;
        lowY = baseY - verticalAmount;

        StartCoroutine(StartHorizontalAfterDelay());
    }

    IEnumerator StartHorizontalAfterDelay() //Delay
    {
        yield return new WaitForSeconds(startDelay);
        moveHorizontally = true;
    }

    void Update()
    {
        
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman); //analiza la musica

        float bass = 0f;
        for (int i = 0; i < bassSamples; i++)
            bass += samples[i];

        float smoothedBass = Mathf.Lerp(prevBass, bass, Time.deltaTime * 10f);
        prevBass = smoothedBass;

        if (smoothedBass > beatThreshold && Time.time - lastBeatTime > beatCooldown) //detectar
        {
            isUp = !isUp;
            lastBeatTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        float targetY = isUp ? baseY : lowY;

        float targetX = rb.position.x;
        if (moveHorizontally)
            targetX -= horizontalSpeed * Time.fixedDeltaTime;

        Vector2 targetPos = new Vector2(targetX, targetY);

        rb.MovePosition(
            Vector2.Lerp(rb.position, targetPos, Time.fixedDeltaTime * verticalSmooth) //mueve
        );
    }
}
