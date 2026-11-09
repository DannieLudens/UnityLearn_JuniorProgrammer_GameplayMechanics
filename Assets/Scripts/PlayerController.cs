using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;

    private InputSystem_Actions _controls;
    private Rigidbody _playerRb;
    private GameObject _focalPoint;
    private float _forwardInput;
    public bool _hasPoweredUp;
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
            _hasPoweredUp = true;
            Destroy(other.gameObject);
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
