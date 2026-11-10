using UnityEngine;
using UnityEngine.SceneManagement;

// Loads scenes by name. Its public methods are hooked to the UI Buttons' OnClick events.
public class SceneLoader : MonoBehaviour
{
    public const string MainMenuScene = "MainMenu"; // Must match the scene name in Build Profiles > Scene List

    // Loads any scene listed in Build Profiles > Scene List (e.g. "Prototype 3", "Challenge 3").
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Shortcut used by the "Menu" buttons.
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
}
