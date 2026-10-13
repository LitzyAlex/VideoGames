using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform humano;
    public Transform maldito;

    [Header("Offset de la cámara")]
    public Vector3 offset = new Vector3(0f, 2f, 0f); // 👈 sube la cámara

    private bool cameraLocked = false;
    private Vector3 lockedPosition;

    void LateUpdate()
    {
        if (cameraLocked) return;

        if (humano.gameObject.activeInHierarchy)
            Seguir(humano);
        else if (maldito.gameObject.activeInHierarchy)
            Seguir(maldito);
    }

    void Seguir(Transform objetivo)
    {
        transform.position = new Vector3(
            objetivo.position.x + offset.x,
            objetivo.position.y + offset.y,
            transform.position.z
        );
    }

    public void LockCamera(Vector3 position)
    {
        cameraLocked = true;
        lockedPosition = position;

        transform.position = new Vector3(
            position.x + offset.x,
            position.y + offset.y,
            transform.position.z
        );
    }

    public void UnlockCamera()
    {
        cameraLocked = false;
    }
}

