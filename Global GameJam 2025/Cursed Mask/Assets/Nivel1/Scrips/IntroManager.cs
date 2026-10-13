using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource musicaZona;
    public AudioSource musicaGlobal;

    private int jugadoresDentro = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadoresDentro++;

            if (jugadoresDentro == 1)
            {
                if (musicaGlobal.isPlaying)
                    musicaGlobal.Stop();

                musicaZona.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadoresDentro--;

            if (jugadoresDentro <= 0)
            {
                musicaZona.Stop();
                musicaGlobal.Play();
            }
        }
    }
}


