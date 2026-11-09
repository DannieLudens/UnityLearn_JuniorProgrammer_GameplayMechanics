using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;            // Chase force (set per wave by SpawnManagerX)
    private Rigidbody enemyRb;     // Enemy's Rigidbody
    private GameObject playerGoal; // Target: the player's goal

    // On start: cache the references it needs
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.FindGameObjectWithTag("Player Goal"); // Finds by tag (safer than by name)
    }

    // Fixed physics step: pushes the enemy toward the player's goal
    private void FixedUpdate()
    {
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized; // (target - origin) = direction; normalized = length 1
        enemyRb.AddForce(lookDirection * speed);                                                 // Continuous force, no deltaTime
    }

    // Called on a physical collision: removes the enemy when it reaches either goal
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy Goal") || other.gameObject.CompareTag("Player Goal")) // || = either one
        {
            Destroy(gameObject);
        }
    }
}
