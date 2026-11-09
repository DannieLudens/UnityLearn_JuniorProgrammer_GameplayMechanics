using UnityEngine;
using System.Collections; // Needed for IEnumerator (coroutines)

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f; // Rolling force (the Inspector value overrides this default)

    public bool hasPoweredUp;           // True while the power-up is active
    public GameObject powerUpIndicator; // Visual ring shown while powered up

    private InputSystem_Actions _controls;     // Own instance of the generated Input System class
    private Rigidbody _playerRb;               // Player's Rigidbody, used to apply forces
    private GameObject _focalPoint;            // Camera pivot: defines which way is "forward"
    private float _forwardInput;               // W/S input, read in Update and used in FixedUpdate
    private float _powerUpStregth = 15f;       // Knockback force applied to enemies while powered up
    private Coroutine _powerUpRoutine;         // Active countdown reference (so it can be restarted)

    // Runs first, before OnEnable: initializes references
    void Awake()
    {
        _controls = new InputSystem_Actions();              // Creates the input actions
        _playerRb = GetComponent<Rigidbody>();              // Gets the Rigidbody on this same GameObject
        _focalPoint = GameObject.Find("Focal Point");       // Finds by name (breaks if renamed)
    }

    // When the object is enabled: start listening to input
    private void OnEnable()
    {
        _controls.Player.Enable(); // Enables the "Player" Action Map
    }

    // Every frame: read input and update visuals (no physics here)
    void Update()
    {
        _forwardInput = _controls.Player.Move.ReadValue<Vector2>().y;                         // .y = forward/back (W/S)
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0); // Ring follows the player, slightly lower
    }

    // Fixed physics step: forces go here (no Time.deltaTime)
    private void FixedUpdate()
    {
        _playerRb.AddForce(_focalPoint.transform.forward * (_forwardInput * playerSpeed)); // Pushes toward where the camera faces
    }

    // Called when entering a collider marked as Is Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPoweredUp = true;
            powerUpIndicator.SetActive(true); // Shows the ring
            Destroy(other.gameObject);        // Removes the collected power-up
            if (_powerUpRoutine != null)
            {
                StopCoroutine(_powerUpRoutine); // Cancels the previous countdown if one is running
            }
            _powerUpRoutine = StartCoroutine(PowerUpCountdownRoutine()); // Starts (or restarts) the 7 seconds
        }
    }

    // Coroutine: waits 7 seconds without blocking the game, then removes the power-up
    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7); // Pauses this method for 7 s; the game keeps running
        hasPoweredUp = false;
        powerUpIndicator.SetActive(false);  // Hides the ring
    }

    // Called on a physical collision (colliders without Is Trigger)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPoweredUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();                               // Rigidbody of the hit enemy
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized; // Player → enemy direction, length 1

            Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to " + hasPoweredUp);

            enemyRb.AddForce(awayFromPlayer * _powerUpStregth, ForceMode.Impulse); // Impulse = instant hit
        }
    }

    // When the object is disabled: stop listening to input
    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    // When the object is destroyed: free the actions created with new
    private void OnDestroy()
    {
        _controls.Dispose();
    }
}
