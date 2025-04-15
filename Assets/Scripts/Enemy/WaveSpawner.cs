using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int enemyCount;
        public GameObject[] enemyPrefabs;
        public float spawnInterval;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;

    private Wave currentWave;
    private int currentWaveIndex;
    private bool canSpawn = true;
    private float nextSpawnTime;
    private int enemiesRemainingToSpawn;
    private int enemiesRemainingAlive;

    void Start()
    {
        if (spawnPoints.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 spawn points");
        }

        StartNextWave();
    }

    void Update()
    {
        if (canSpawn && enemiesRemainingToSpawn > 0 && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (enemiesRemainingToSpawn == 0)
            {
                canSpawn = false;
            }
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemyPrefab = currentWave.enemyPrefabs[Random.Range(0, currentWave.enemyPrefabs.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
        if (enemyScript != null)
        {
            enemyScript.OnDeath += OnEnemyDeath;
        }
    }

    

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if (currentWaveIndex >= waves.Length && enemiesRemainingAlive == 0)
        {
            Debug.Log("All waves completed!");
        }
        else if (enemiesRemainingToSpawn == 0 && enemiesRemainingAlive == 0)
        {
            Invoke("StartNextWave", timeBetweenWaves);
        }
    }

    void StartNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            currentWave = waves[currentWaveIndex];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = currentWave.enemyCount;
            canSpawn = true;

            Debug.Log("Starting Wave: " + currentWave.waveName);
            currentWaveIndex++;
        }
    }

    public int GetCurrentWaveNumber()
    {
        return currentWaveIndex;
    }

    public int GetTotalWaves()
    {
        return waves.Length;
    }
}
