using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // Enemy prefab (assigned in the Inspector)
    public int enemyCount;            // Number of enemies currently in the scene
    public int waveNumber = 1;           // Current wave number (starts at 1)
    private float _spawnRange = 9f; // Max distance from the center where enemies can spawn

    // On start: spawns one enemy at a random position
    void Start()
    {
        SpawnEnemyWave(waveNumber);
    }

    private void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length; // Counts all Enemy instances in the scene
        if (enemyCount == 0) // If all enemies are gone
        {
            waveNumber++; // Increase the wave number
            SpawnEnemyWave(waveNumber); // Spawn a new wave of 1 enemy
        }
    }

    // Returns a random position on the island (Y = 0)
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange); // Float Random.Range: both ends inclusive
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    // Spawns a wave of enemies
    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
