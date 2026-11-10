using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.Events;  // UnityEventTools (persistent OnClick listeners)
using UnityEngine;
using UnityEngine.UI;

namespace MenuKit.Editor
{
    // Editor-only tool: builds the MenuKit prefabs from code. Menu: Tools > MenuKit > Build Prefabs.
    // Running it again rebuilds (overwrites) the prefabs.
    public static class MenuKitPrefabBuilder
    {
        private const string PrefabFolder = "Assets/MenuKit/Prefabs"; // Where the prefabs are saved

        private static readonly Color BackgroundColor = new Color(0.07f, 0.11f, 0.2f); // Dark navy
        private static readonly Color HintColor = new Color(1f, 1f, 1f, 0.6f);         // Semi-transparent white

        // Menu item: builds all three prefabs
        [MenuItem("Tools/MenuKit/Build Prefabs")]
        public static void BuildPrefabs()
        {
            Directory.CreateDirectory(PrefabFolder);

            Button buttonTemplate = BuildMenuButton(); // Built first: the main menu needs it as a template
            BuildMainMenu(buttonTemplate);
            BuildLevelMenu();

            AssetDatabase.SaveAssets();
            Debug.Log("MenuKit: prefabs built in " + PrefabFolder);
        }

        // Template button used by MainMenuController to create its buttons
        private static Button BuildMenuButton()
        {
            GameObject root = CreateButtonObject("MenuButton", "Button", 46);
            LayoutElement layout = root.AddComponent<LayoutElement>(); // Size inside the Vertical Layout Group
            layout.preferredWidth = 640;
            layout.preferredHeight = 110;

            GameObject prefab = SavePrefab(root, "MenuButton");
            return prefab.GetComponent<Button>();
        }

        // Full main menu: canvas, background, texts, button container and controller
        private static void BuildMainMenu(Button buttonTemplate)
        {
            GameObject root = CreateCanvas("MainMenu");

            Image background = CreateUIObject("Background", root.transform).AddComponent<Image>();
            background.color = BackgroundColor;
            Stretch(background.rectTransform); // Fills the whole screen

            TMP_Text title = CreateText("Title", root.transform, "Junior Programmer", 96, FontStyles.Bold, Color.white);
            Place(title.rectTransform, new Vector2(0, 330), new Vector2(1600, 140));

            TMP_Text subtitle = CreateText("Subtitle", root.transform, "Unit X", 48, FontStyles.Normal, Color.white);
            Place(subtitle.rectTransform, new Vector2(0, 220), new Vector2(1600, 80));

            GameObject buttons = CreateUIObject("Buttons", root.transform);
            Place(buttons.GetComponent<RectTransform>(), new Vector2(0, -40), new Vector2(800, 380));
            VerticalLayoutGroup group = buttons.AddComponent<VerticalLayoutGroup>(); // Stacks buttons automatically
            group.spacing = 30;
            group.childAlignment = TextAnchor.MiddleCenter;
            group.childControlWidth = true;       // Layout sets each button's size from its LayoutElement
            group.childControlHeight = true;
            group.childForceExpandWidth = false;  // Use the preferred size, don't stretch to fill
            group.childForceExpandHeight = false;

            TMP_Text hint = CreateText("Hint", root.transform, "Press Esc in any level to return here", 34, FontStyles.Italic, HintColor);
            Place(hint.rectTransform, new Vector2(0, -400), new Vector2(1600, 60));

            // Wire the controller to its own children (references inside a prefab survive export)
            MainMenuController controller = root.AddComponent<MainMenuController>();
            SerializedObject so = new SerializedObject(controller);
            so.FindProperty("titleText").objectReferenceValue = title;
            so.FindProperty("subtitleText").objectReferenceValue = subtitle;
            so.FindProperty("hintText").objectReferenceValue = hint;
            so.FindProperty("buttonTemplate").objectReferenceValue = buttonTemplate;
            so.FindProperty("buttonParent").objectReferenceValue = buttons.transform;
            so.ApplyModifiedPropertiesWithoutUndo();

            SavePrefab(root, "MainMenu");
        }

        // In-level overlay: small "Menu (Esc)" button in the top-left corner
        private static void BuildLevelMenu()
        {
            GameObject root = CreateCanvas("LevelMenu");
            root.GetComponent<Canvas>().sortingOrder = 100; // Draw on top of other canvases

            LevelMenuController controller = root.AddComponent<LevelMenuController>();

            GameObject buttonObject = CreateButtonObject("MenuButton", "Menu (Esc)", 32);
            buttonObject.transform.SetParent(root.transform, false);
            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1); // Anchored to the top-left corner
            rect.anchoredPosition = new Vector2(30, -30);
            rect.sizeDelta = new Vector2(240, 70);

            Button button = buttonObject.GetComponent<Button>();
            UnityEventTools.AddPersistentListener(button.onClick, controller.GoToMenu); // Saved OnClick, visible in the Inspector

            SavePrefab(root, "LevelMenu");
        }

        // ---------- Helpers ----------

        // Screen-space canvas that scales with the window (important for Web builds)
        private static GameObject CreateCanvas(string name)
        {
            GameObject root = new GameObject(name, typeof(RectTransform));
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            root.AddComponent<GraphicRaycaster>(); // Lets the canvas receive clicks
            return root;
        }

        // Button with an Image background and a centered TMP label
        private static GameObject CreateButtonObject(string name, string label, float fontSize)
        {
            GameObject root = new GameObject(name, typeof(RectTransform));
            Image image = root.AddComponent<Image>();
            image.color = new Color(0.25f, 0.45f, 0.95f);
            Button button = root.AddComponent<Button>();
            button.targetGraphic = image;

            TMP_Text text = CreateText("Label", root.transform, label, fontSize, FontStyles.Bold, Color.white);
            Stretch(text.rectTransform);                 // Label fills the button
            text.rectTransform.offsetMin = new Vector2(20, 8);   // Inner padding (left, bottom)
            text.rectTransform.offsetMax = new Vector2(-20, -8); // Inner padding (right, top)
            text.enableAutoSizing = true;                // Long labels shrink to fit on one line
            text.fontSizeMin = 24;
            text.fontSizeMax = fontSize;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            return root;
        }

        // Empty UI object with a RectTransform
        private static GameObject CreateUIObject(string name, Transform parent)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        // Centered TextMeshPro label using the project's default TMP font
        private static TMP_Text CreateText(string name, Transform parent, string content, float size, FontStyles style, Color color)
        {
            TextMeshProUGUI text = CreateUIObject(name, parent).AddComponent<TextMeshProUGUI>();
            text.font = TMP_Settings.defaultFontAsset; // Requires TMP Essentials imported in the project
            text.text = content;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = TextAlignmentOptions.Center;
            text.raycastTarget = false; // Labels shouldn't block clicks on buttons
            return text;
        }

        // Anchors a RectTransform to all four edges of its parent
        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        // Centers a RectTransform with a position offset and size
        private static void Place(RectTransform rect, Vector2 position, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        // Saves the temporary object as a prefab asset and deletes the temporary object
        private static GameObject SavePrefab(GameObject root, string name)
        {
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, $"{PrefabFolder}/{name}.prefab");
            Object.DestroyImmediate(root);
            return prefab;
        }
    }
}
