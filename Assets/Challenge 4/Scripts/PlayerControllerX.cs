using System.Collections; // Needed for IEnumerator (coroutines)
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    [SerializeField] private float turboStrength = 15f; // Impulse force applied once per Space press
    [SerializeField] private float turboCooldown = 1f;  // Seconds to wait before the turbo can be used again

    private float _nextTurboTime;  // Time.time when the turbo becomes available again
    private Rigidbody playerRb;    // Player's Rigidbody, used to apply forces
    private float speed = 10;      // Rolling force (continuous, no deltaTime)
    private GameObject focalPoint; // Camera pivot: defines which way is "forward"

    public ParticleSystem turboSmokeParticle; // Smoke effect played on turbo (child of Focal Point)
    public bool hasPowerup;                   // True while the power-up is active
    public GameObject powerupIndicator;       // Visual ring shown while powered up
    public int powerUpDuration = 5;           // Power-up length in seconds

    private float normalStrength = 10;  // How hard to hit an enemy without power-up
    private float powerupStrength = 25; // How hard to hit an enemy with power-up

    private InputSystem_Actions controls; // Own instance of the generated Input System class
    private Coroutine _powerUpRoutine;    // Active countdown reference (so it can be restarted)

    // Runs first, before OnEnable: creates the input actions
    void Awake()
    {
        controls = new InputSystem_Actions();
    }

    // When the object is enabled: start listening to input
    void OnEnable()
    {
        controls.Player.Enable();
    }

    // On start: cache the references it needs
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point"); // Finds by name (breaks if renamed)
    }

    // Every frame: visuals and one-shot button input
    void Update()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0); // Ring follows the player, slightly lower

        if (controls.Player.Jump.WasPressedThisFrame() && Time.time >= _nextTurboTime) // Space pressed this frame AND cooldown over
        {
            playerRb.AddForce(focalPoint.transform.forward * turboStrength, ForceMode.Impulse); // Instant push toward camera forward
            turboSmokeParticle.Play();                                                         // Fires the smoke effect once
            _nextTurboTime = Time.time + turboCooldown;                                        // Locks the turbo for turboCooldown seconds
        }
    }

    // Fixed physics step: continuous movement force
    private void FixedUpdate()
    {
        float verticalInput = controls.Player.Move.ReadValue<Vector2>().y;         // .y = forward/back (W/S)
        playerRb.AddForce(focalPoint.transform.forward * (verticalInput * speed)); // Pushes toward where the camera faces
    }

    // Called when entering a trigger collider: collects the power-up
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject); // Removes the collected power-up
            hasPowerup = true;
            if (_powerUpRoutine != null)
            {
                StopCoroutine(_powerUpRoutine); // Cancels the previous countdown if one is running
            }
            _powerUpRoutine = StartCoroutine(PowerupCooldown()); // Starts (or restarts) the countdown
            powerupIndicator.SetActive(true);                   // Shows the ring
        }
    }

    // Coroutine: waits powerUpDuration seconds without blocking the game, then removes the power-up
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false); // Hides the ring
    }

    // Called on a physical collision: knocks enemies away
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();                         // Rigidbody of the hit enemy
            Vector3 awayFromPlayer = (other.gameObject.transform.position - transform.position).normalized; // Player → enemy direction, length 1

            if (hasPowerup) // Powered up: strong hit
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // No power-up: normal hit
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }

    // When the object is disabled: stop listening to input
    void OnDisable()
    {
        controls.Player.Disable();
    }

    // When the object is destroyed: free the actions created with new
    void OnDestroy()
    {
        controls.Dispose();
    }
}
