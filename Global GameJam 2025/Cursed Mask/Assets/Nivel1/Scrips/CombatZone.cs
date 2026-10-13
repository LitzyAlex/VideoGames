using UnityEngine;
using System.Collections;

public class CombatZone : MonoBehaviour
{
    public CamaraFollow cameraFollow;
    public Transform cameraLockPoint;

    [Header("Enemigos")]
    public GameObject[] enemiesToActivate;

    [Header("Spawn")]
    public float timeBetweenSpawns = 1.5f;

    private bool activated = false;
    private bool spawningFinished = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return;

        if (collision.CompareTag("Player"))
        {
            activated = true;

            
            cameraFollow.LockCamera(cameraLockPoint.position);

            
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        foreach (GameObject enemy in enemiesToActivate)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }

        spawningFinished = true;
    }

    private void Update()
    {
        if (!activated || !spawningFinished) return;

        bool allDead = true;

        foreach (GameObject enemy in enemiesToActivate)
        {
            if (enemy != null)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            cameraFollow.UnlockCamera();
            Destroy(gameObject);
        }
    }
}
