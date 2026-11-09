using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // Force used to chase the player
    private Rigidbody _enemyRb;                // Enemy's Rigidbody
    private GameObject _player;                // Player being chased

    // On start: cache the references it needs
    void Start()
    {
        _enemyRb = GetComponent<Rigidbody>();
        _player = GameObject.Find("Player"); // Finds by name (prefabs can't store scene references)
    }

    private void Update()
    {
        if (transform.position.y < -10) // If the enemy falls off the island
        {
            Destroy(gameObject); // Remove it from the scene
        }
    }

    // Fixed physics step: pushes the enemy toward the player
    private void FixedUpdate()
    {
        Vector3 lookDirection = (_player.transform.position - transform.position).normalized; // (target - origin) = vector direction; normalized = length 1
        _enemyRb.AddForce(lookDirection * speed);                                            // Same force regardless of distance
    }
}
