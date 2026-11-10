using UnityEngine;
using UnityEngine.InputSystem; // New Input System (provides InputAction)
using UnityEngine.SceneManagement;

// Returns to the main menu from a level, with the on-screen "Menu" button or the Esc key.
public class BackToMenu : MonoBehaviour
{
    // Same idea as jumpAction in PlayerController, but the binding is set in code so no Inspector setup is needed.
    private readonly InputAction _backAction = new InputAction("Back", binding: "<Keyboard>/escape");

    private void OnEnable()
    {
        _backAction.Enable();   // InputActions start disabled; enable to receive input
    }

    private void OnDisable()
    {
        _backAction.Disable();  // Stop listening when this object is disabled or the scene unloads
    }

    // Called every frame: checks for the Esc key.
    private void Update()
    {
        if (_backAction.triggered)
        {
            GoToMenu();
        }
    }

    // Hooked to the "Menu" button's OnClick event.
    public void GoToMenu()
    {
        SceneManager.LoadScene(SceneLoader.MainMenuScene);
    }
}
