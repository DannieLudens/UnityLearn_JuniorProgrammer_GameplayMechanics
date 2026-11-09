using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject powerupPrefab; // Power-up prefab (assigned in the Inspector)
    public GameObject enemyPrefab;   // Enemy prefab (assigned in the Inspector)
    [SerializeField] private int enemyCount;           // Enemies currently in the scene (visible in Inspector for debugging)
    [SerializeField] private int waveNumber = 1;       // Current wave; also how many enemies it spawns
    private float _spawnRange = 9f;  // Max distance from the center where things can spawn

    // On start: spawns the first wave and one power-up
    void Start()
    {
        SpawnEnemyWave(waveNumber);                                                            // Wave 1 = 1 enemy
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation); // One power-up per wave
    }

    // Every frame: checks if the wave is cleared and starts the next one
    private void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length; // Counts every object with an Enemy component
        if (enemyCount == 0) // All enemies are gone
        {
            waveNumber++;                                                                          // Next wave has one more enemy
            SpawnEnemyWave(waveNumber);                                                            // Spawns waveNumber enemies
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation); // One power-up per wave
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

    // Spawns the given number of enemies at random positions
    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++) // Start at 0; repeat while i < enemiesToSpawn; add 1 each loop
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
