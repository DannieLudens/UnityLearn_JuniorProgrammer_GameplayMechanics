using TMPro;                            // TextMeshPro text components
using UnityEngine;
using UnityEngine.EventSystems;        // EventSystem (needed for UI clicks)
using UnityEngine.InputSystem.UI;      // InputSystemUIInputModule (new Input System UI module)
using UnityEngine.SceneManagement;     // SceneManager
using UnityEngine.UI;                  // Button, Image

namespace MenuKit // Namespace avoids name clashes with scripts already in the project
{
    // One menu entry: a button label and the scene it loads. Filled in the Inspector.
    [System.Serializable]
    public class MenuEntry
    {
        public string label = "Play";                            // Text shown on the button
        public string sceneName = "SampleScene";                 // Must match a scene in the Build Profile list
        public Color color = new Color(0.25f, 0.45f, 0.95f);     // Button background color
    }

    // Builds the main menu buttons from a list at runtime, so each project only edits the Inspector.
    public class MainMenuController : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private string title = "Junior Programmer"; // Big title at the top
        [SerializeField] private string subtitle = "Unit X";         // Smaller text under the title
        [SerializeField] private string hint = "Press Esc in any level to return here"; // Footer text

        [Header("Buttons")]
        [SerializeField] private MenuEntry[] entries; // One button per entry

        [Header("References (already wired in the prefab)")]
        [SerializeField] private TMP_Text titleText;    // Title label inside the prefab
        [SerializeField] private TMP_Text subtitleText; // Subtitle label inside the prefab
        [SerializeField] private TMP_Text hintText;     // Hint label inside the prefab
        [SerializeField] private Button buttonTemplate; // Button prefab used as a template
        [SerializeField] private Transform buttonParent; // Object with the Vertical Layout Group

        // Runs first: makes sure clicks work even if the scene has no EventSystem
        private void Awake()
        {
            EnsureEventSystem();
        }

        // On start: fills the texts and creates one button per entry
        private void Start()
        {
            titleText.text = title;
            subtitleText.text = subtitle;
            hintText.text = hint;

            if (entries == null || entries.Length == 0)
            {
                Debug.LogWarning("MenuKit: add at least one entry in the MainMenuController Inspector."); // Reminder, not an error
                return;
            }

            foreach (MenuEntry entry in entries) // Same idea as a for loop, one pass per entry
            {
                CreateButton(entry);
            }
        }

        // Creates one button from the template and hooks its click to LoadScene
        private void CreateButton(MenuEntry entry)
        {
            Button button = Instantiate(buttonTemplate, buttonParent);  // Instantiate returns the new copy
            button.GetComponentInChildren<TMP_Text>().text = entry.label;
            if (entry.color.a > 0f) // New list items can start fully transparent: keep the template color then
            {
                button.GetComponent<Image>().color = entry.color;
            }

            string sceneToLoad = entry.sceneName;                        // Local copy captured by the lambda below
            button.onClick.AddListener(() => LoadScene(sceneToLoad));    // Hooks the click from code (no Inspector OnClick needed)
        }

        // Loads any scene listed in the Build Profile
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        // Creates an EventSystem only if the scene doesn't have one (avoids duplicates)
        public static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() != null) return; // Scene already has one: do nothing

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>(); // UI module for the new Input System
        }
    }
}
