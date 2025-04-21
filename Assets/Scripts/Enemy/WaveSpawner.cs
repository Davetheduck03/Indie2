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

    public GameObject[] Wall;
    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;

    private Wave currentWave;
    private int currentWaveIndex;
    private bool canSpawn = false;
    private float nextSpawnTime;
    private int enemiesRemainingToSpawn;
    private int enemiesRemainingAlive;
    private bool isSpawning = false;

    void Update()
    {
        if (isSpawning && canSpawn && enemiesRemainingToSpawn > 0 && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (enemiesRemainingToSpawn == 0)
            {
                canSpawn = false;
            }
        }
        if (isSpawning && currentWaveIndex >= waves.Length && enemiesRemainingAlive == 0)
        {
            CompleteAllWaves();
        }
    }

    void CompleteAllWaves()
    {
        Wall[0].SetActive(false);
        isSpawning = false;
        Wall[1].SetActive(false);
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            Wall[0].SetActive(true);
            isSpawning = true;
            currentWaveIndex = 0;
            StartNextWave();
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(StartNextWave));
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
        else
        {
            Debug.LogWarning("Enemy prefab missing Enemy script");
        }
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if (isSpawning)
        {
            if (currentWaveIndex >= waves.Length && enemiesRemainingAlive == 0)
            {
                Debug.Log("All waves completed!");
            }
            else if (enemiesRemainingToSpawn == 0 && enemiesRemainingAlive == 0)
            {
                Invoke(nameof(StartNextWave), timeBetweenWaves);
            }
        }
    }

    void StartNextWave()
    {
        if (isSpawning && currentWaveIndex < waves.Length)
        {
            currentWave = waves[currentWaveIndex];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = currentWave.enemyCount;
            canSpawn = true;

            Debug.Log("Starting Wave: " + currentWave.waveName);
            currentWaveIndex++;
        }

    }

    public int GetCurrentWaveNumber() => currentWaveIndex;
    public int GetTotalWaves() => waves.Length;
    public bool IsSpawningActive() => isSpawning;
}
