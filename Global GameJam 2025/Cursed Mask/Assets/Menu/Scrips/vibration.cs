using UnityEngine;

public class vibration : MonoBehaviour
{
    public float height = 10f;
    public float speed = 2f;

    private Vector3 startPos;
    private bool goingUp = true;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        if (goingUp)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                startPos + Vector3.up * height,
                step
            );

            if (transform.localPosition.y >= startPos.y + height - 0.01f)
                goingUp = false;
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                startPos,
                step
            );

            if (transform.localPosition.y <= startPos.y + 0.01f)
                goingUp = true;
        }
    }
}

