using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float minX = -10f;
    public float maxX = 10f;

    public float minY = -5f;
    public float maxY = 5f;

    [Header("Zoom Game Over")]
    public float zoomNormal = 5f;
    public float zoomGameOver = 2.5f;
    public float velocidadZoom = 2f;

    private bool gameOver = false;

    private Camera camara;

    void Start()
    {
        camara = GetComponent<Camera>();
        camara.orthographicSize = zoomNormal;
    }

    void LateUpdate()
    {
        float x;
        float y;

        if (gameOver)
        {
            // Durante Game Over, la cámara se centra completamente en el jugador
            x = player.position.x;
            y = player.position.y;
        }
        else
        {
            // Movimiento normal con límites
            x = Mathf.Clamp(player.position.x, minX, maxX);
            y = Mathf.Clamp(player.position.y, minY, maxY);
        }

        transform.position = new Vector3(x, y, transform.position.z);

        // Acercamiento durante Game Over
        if (gameOver)
        {
            camara.orthographicSize = Mathf.Lerp(
                camara.orthographicSize,
                zoomGameOver,
                velocidadZoom * Time.unscaledDeltaTime
            );
        }
    }

    public void ActivarGameOver()
    {
        gameOver = true;
    }
}