using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // Enemy prefab (assigned in the Inspector)
    private float _spawnRange = 9f; // Max distance from the center where enemies can spawn

    // On start: spawns one enemy at a random position
    void Start()
    {
        Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation); // Creates a copy of the prefab
    }

    // Returns a random position on the island (Y = 0)
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange); // Float Random.Range: both ends inclusive
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}
