using UnityEngine;

public class RotateCameraX : MonoBehaviour
{
    private float speed = 200;            // Rotation speed in degrees per second
    public GameObject player;             // Player to follow (assigned in the Inspector)
    private InputSystem_Actions controls; // Own instance of the generated Input System class

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

    // Every frame: rotates the camera pivot and keeps it on the player
    void Update()
    {
        float horizontalInput = controls.Player.Move.ReadValue<Vector2>().x;   // .x = left/right (A/D)
        transform.Rotate(Vector3.up, horizontalInput * speed * Time.deltaTime); // Rotates around Y; deltaTime = frame-rate independent

        transform.position = player.transform.position; // Follows the player's position only (it doesn't roll)
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
