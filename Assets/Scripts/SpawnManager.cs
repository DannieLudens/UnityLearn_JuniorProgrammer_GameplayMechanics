using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    private float _spawnRange = 9f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomX = Random.Range(-_spawnRange, _spawnRange);
        float randomZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);
        Instantiate(enemyPrefab, randomPosition, enemyPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
