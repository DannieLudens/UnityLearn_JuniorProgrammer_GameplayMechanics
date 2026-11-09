using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    /*public float rotationSpeed;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }*/

    [SerializeField] private float rotationSpeed = 150f;
    private InputSystem_Actions _controls;

    private void Awake()
    {
        _controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        // Debug.Log(_controls.Player.Move);
    }

    private void Update()
    {
        Vector2 moveInput = _controls.Player.Move.ReadValue<Vector2>();
        float horizontalInput = moveInput.x;
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
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
