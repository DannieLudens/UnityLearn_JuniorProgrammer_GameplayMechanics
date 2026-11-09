using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;

    private InputSystem_Actions _controls;
    private Rigidbody _playerRb;
    private GameObject _focalPoint;
    private float _forwardInput;
    private float _powerUpStregth = 15f;

    public bool hasPoweredUp;

    void Awake()
    {
        _controls = new InputSystem_Actions();
        _playerRb = GetComponent<Rigidbody>();
        _focalPoint = GameObject.Find("Focal Point");
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }

    void Update()
    {
        _forwardInput = _controls.Player.Move.ReadValue<Vector2>().y;

    }

    private void FixedUpdate()
    {
        _playerRb.AddForce(_focalPoint.transform.forward * (_forwardInput * playerSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPoweredUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
        }
    }

    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPoweredUp = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPoweredUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to " + hasPoweredUp);

            enemyRb.AddForce(awayFromPlayer * _powerUpStregth, ForceMode.Impulse);
        }
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    private void OnDestroy()
    {
        _controls.Dispose();
    }
}
