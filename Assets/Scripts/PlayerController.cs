using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 150f;

    private InputSystem_Actions _controls;
    private Rigidbody _playerRb;
    private GameObject _focalPoint;
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
        Vector2 moveInput = _controls.Player.Move.ReadValue<Vector2>();
        float forwardInput = moveInput.y;
        _playerRb.AddForce(_focalPoint.transform.forward * (forwardInput * playerSpeed * Time.deltaTime));
    }
}
