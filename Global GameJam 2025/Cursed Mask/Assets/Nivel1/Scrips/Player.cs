using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject humano;
    public GameObject maldito;

    [Header("Tiempo")]
    public float tiempoConMascara = 5f;

    [Header("Intentos")]
    public int intentosNecesarios = 3;

    private float temporizador;
    private bool tieneMascara = true;
    private int intentosActuales = 0;

    void Start()
    {
        ActivarHumano();
    }

    void Update()
    {
        if (tieneMascara)
        {
            temporizador -= Time.deltaTime;

            if (temporizador <= 0)
            {
                QuitarMascara();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                IntentarPonerMascara();
            }
        }
    }

    void QuitarMascara()
    {
        tieneMascara = false;
        intentosActuales = 0;

        humano.SetActive(false);
        maldito.SetActive(true);

        // Mantener posición
        maldito.transform.position = humano.transform.position;
        Animator anim = maldito.GetComponent<Animator>();
        anim.Rebind();
        anim.Update(0f);

        Debug.Log("Se quitó la máscara");
    }

    void IntentarPonerMascara()
    {
        intentosNecesarios = Random.Range(1, 11);
        intentosActuales++;
        Debug.Log("Intento " + intentosActuales);

        if (intentosActuales >= intentosNecesarios)
        {
            ActivarHumano();
        }
    }

    void ActivarHumano()
    {
        tieneMascara = true;
        temporizador = tiempoConMascara;

        maldito.SetActive(false);
        humano.SetActive(true);

        humano.transform.position = maldito.transform.position;

        Debug.Log("Máscara puesta, vuelve humano");
    }
}

