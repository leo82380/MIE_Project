using UnityEditor;
using UnityEditor.SceneManagement;

namespace MIE.Editor.StartInTitle
{
    [InitializeOnLoad]
    public static class StartInTitleEditor
    {
        private static readonly string titleSceneName = "Title";
        private const string PrefKey = "MIE.StartInTitle.isEnabled";
        private static bool isEnabled;
        public static bool IsEnabled => isEnabled;
        public static void Toggle()
        {
            isEnabled = !isEnabled;
            EditorPrefs.SetBool(PrefKey, isEnabled);
        }

        static StartInTitleEditor()
        {
            isEnabled = EditorPrefs.GetBool(PrefKey, true);
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private const string LastScenePrefKey = "MIE.StartInTitle.lastScenePath";
        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!isEnabled) return;
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                EditorPrefs.SetString(LastScenePrefKey, currentScene.path);
                if (currentScene.name != titleSceneName)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene($"Assets/00.Scenes/{titleSceneName}.unity");
                    }
                }
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                string lastScenePath = EditorPrefs.GetString(LastScenePrefKey, null);
                if (!string.IsNullOrEmpty(lastScenePath) && lastScenePath != $"Assets/00.Scenes/{titleSceneName}.unity")
                {
                    EditorSceneManager.OpenScene(lastScenePath);
                }
            }
        }
    }
}
