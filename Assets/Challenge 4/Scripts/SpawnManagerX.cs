using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    [SerializeField] private float enemyBaseSpeed = 3f;         // Enemy speed in wave 1
    [SerializeField] private float speedIncreasePerWave = 0.5f; // Extra enemy speed added each new wave

    public GameObject enemyPrefab;   // Enemy prefab (assigned in the Inspector)
    public GameObject powerupPrefab; // Power-up prefab (assigned in the Inspector)

    private float spawnRangeX = 10; // Max X distance from the center
    private float spawnZMin = 15;   // Min spawn Z (enemy side of the field)
    private float spawnZMax = 25;   // Max spawn Z
    private Rigidbody _playerRb;    // Cached player Rigidbody (avoids repeated GetComponent)

    public int enemyCount;    // Enemies currently in the scene
    public int waveCount = 1; // Current wave; also how many enemies it spawns

    public GameObject player; // Player reference (assigned in the Inspector)

    // On start: cache the player's Rigidbody once
    private void Start()
    {
        _playerRb = player.GetComponent<Rigidbody>();
    }

    // Every frame: starts a new wave when all enemies are gone
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length; // Counts objects tagged "Enemy"

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    // Returns a random position on the enemy side of the field
    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    // Spawns a power-up (if none left) and the given number of enemies, then resets the player
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // Moves power-ups to the player's side

        if (GameObject.FindGameObjectsWithTag("PowerUp").Length == 0) // Only spawn one if none remain
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        for (int i = 0; i < enemiesToSpawn; i++) // One enemy per wave number
        {
            GameObject enemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation); // Keep a reference to the new copy
            enemy.GetComponent<EnemyX>().speed = enemyBaseSpeed + (waveCount - 1) * speedIncreasePerWave;       // Faster each wave (wave 1 = base speed)
        }

        waveCount++;           // Runs after the loop, so the formula above uses (waveCount - 1)
        ResetPlayerPosition(); // Puts the player back at the start
    }

    // Moves the player in front of its own goal and stops all its motion
    void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);
        _playerRb.linearVelocity = Vector3.zero;  // Stops movement
        _playerRb.angularVelocity = Vector3.zero; // Stops rolling
    }
}
