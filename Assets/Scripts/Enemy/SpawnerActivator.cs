using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerActivator : MonoBehaviour
{
    public GameObject Spawner;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            WaveSpawner spawner = Spawner.GetComponent<WaveSpawner>();
            spawner.StartSpawning();
            Destroy(gameObject);
        }
    }
}

