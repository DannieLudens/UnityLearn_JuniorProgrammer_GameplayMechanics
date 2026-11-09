using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // Original course version (old Input Manager), kept for reference:
    /*public float rotationSpeed;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }*/

    [SerializeField] private float rotationSpeed = 30f; // Degrees per second (the Inspector value overrides this default)
    private InputSystem_Actions _controls;               // Own instance of the generated Input System class

    // Runs first, before OnEnable: creates the input actions
    private void Awake()
    {
        _controls = new InputSystem_Actions();
    }

    // When the object is enabled: start listening to input
    private void OnEnable()
    {
        _controls.Player.Enable(); // Enables the "Player" Action Map
        // Debug.Log(_controls.Player.Move);
    }

    // Every frame: rotates the Focal Point (and its child camera with it)
    private void Update()
    {
        Vector2 moveInput = _controls.Player.Move.ReadValue<Vector2>();                // Vector2 from the Move action
        float horizontalInput = moveInput.x;                                           // .x = left/right (A/D)
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime); // Rotates around Y; deltaTime = frame-rate independent
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
