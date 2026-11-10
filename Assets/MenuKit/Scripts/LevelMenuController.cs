using UnityEngine;
using UnityEngine.InputSystem;     // InputAction (new Input System)
using UnityEngine.SceneManagement; // SceneManager

namespace MenuKit // Namespace avoids name clashes with scripts already in the project
{
    // Goes back to the main menu from a level, with the on-screen button or the Esc key.
    public class LevelMenuController : MonoBehaviour
    {
        [SerializeField] private string menuSceneName = "MainMenu"; // Must match the menu scene name in the Build Profile

        // Binding set in code, so it works in any project without an .inputactions asset
        private readonly InputAction _backAction = new InputAction("Back", binding: "<Keyboard>/escape");

        // Runs first: makes sure the "Menu" button can be clicked even if the scene has no EventSystem
        private void Awake()
        {
            MainMenuController.EnsureEventSystem();
            _backAction.AddBinding("<Gamepad>/start"); // Extra binding: gamepad Start button
        }

        // When the object is enabled: start listening to input
        private void OnEnable()
        {
            _backAction.Enable();
        }

        // Every frame: goes back to the menu when Esc (or Start) is pressed
        private void Update()
        {
            if (_backAction.WasPressedThisFrame())
            {
                GoToMenu();
            }
        }

        // Hooked to the "Menu" button's OnClick inside the prefab
        public void GoToMenu()
        {
            SceneManager.LoadScene(menuSceneName);
        }

        // When the object is disabled: stop listening to input
        private void OnDisable()
        {
            _backAction.Disable();
        }

        // When the object is destroyed: free the action created in code
        private void OnDestroy()
        {
            _backAction.Dispose();
        }
    }
}
