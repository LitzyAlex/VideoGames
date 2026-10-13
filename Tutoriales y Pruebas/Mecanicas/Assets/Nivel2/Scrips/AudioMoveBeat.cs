using UnityEngine;

public class AudioMoveBeat : MonoBehaviour
{
    public AudioSource audioSource;

    public float moveAmount = 2f;
    public float smoothSpeed = 6f;

    public int bassSamples = 8;
    public float beatThreshold = 0.03f;
    public float beatCooldown = 0.3f;

    float baseY;
    float lowY;
    float lastBeatTime;
    float prevBass;
    public bool startUp = true;

    bool isUp = true; // arriba
    float[] samples = new float[64];

    void Start()
    {
        baseY = transform.position.y;
        lowY = baseY - moveAmount;
        isUp = startUp;
    }

    void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        float bassIntensity = 0f;
        for (int i = 0; i < bassSamples; i++)
            bassIntensity += samples[i];

        float smoothedBass = Mathf.Lerp(prevBass, bassIntensity, Time.deltaTime * 10f);
        prevBass = smoothedBass;

        if (smoothedBass > beatThreshold && Time.time - lastBeatTime > beatCooldown)
        {
            isUp = !isUp; // cambia
            lastBeatTime = Time.time;
        }

        float targetY = isUp ? baseY : lowY;

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(transform.position.x, targetY, transform.position.z),
            Time.deltaTime * smoothSpeed
        );
    }
}
