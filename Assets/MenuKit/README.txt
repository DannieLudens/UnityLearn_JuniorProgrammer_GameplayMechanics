MenuKit - reusable main menu + "back to menu" overlay
=====================================================

Requirements
- Unity 6, Input System package, uGUI (includes TextMeshPro)
- TMP Essentials imported (Window > TextMeshPro > Import TMP Essential Resources)

Contents
- Scripts/MainMenuController.cs   Builds the menu buttons from a list in the Inspector
- Scripts/LevelMenuController.cs  Esc key / gamepad Start / on-screen button -> back to menu
- Editor/MenuKitPrefabBuilder.cs  Tools > MenuKit > Build Prefabs (creates/rebuilds the prefabs)
- Prefabs/MainMenu, LevelMenu, MenuButton

Neither prefab contains an EventSystem: both scripts create one at runtime
only if the scene has none (no duplicate warnings).

Use in a new project
1. Assets > Import Package > Custom Package > MenuKit.unitypackage
   (export ONLY the Assets/MenuKit folder, nothing from Course Library)
2. If the Prefabs folder is empty or broken: Tools > MenuKit > Build Prefabs
3. Create a scene named "MainMenu", drag Prefabs/MainMenu into it
4. On the MainMenu object, fill Title, Subtitle and Entries (label + scene name + color)
5. Drag Prefabs/LevelMenu into every level scene
6. Build Profiles > Scene List: MainMenu at index 0, then every level

Notes
- Scene names in Entries must match the Scene List exactly (case and spaces).
- No Quit button on purpose: Application.Quit() does nothing in Web builds.
