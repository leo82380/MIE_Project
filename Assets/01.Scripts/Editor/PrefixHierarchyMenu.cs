using UnityEngine;
using UnityEditor;

namespace MIE.Editor
{
    public class PrefixTools : EditorWindow
    {
        private static readonly string[] prefixes = { "Group_", "Image_", "Button_", "Text_", "Scroll_" };

        [MenuItem("MIE/Change Prefix/Group_", false, 0)]
        private static void SetGroupPrefix() => ApplyPrefix("Group_");

        [MenuItem("MIE/Change Prefix/Image_", false, 1)]
        private static void SetImagePrefix() => ApplyPrefix("Image_");

        [MenuItem("MIE/Change Prefix/Button_", false, 2)]
        private static void SetButtonPrefix() => ApplyPrefix("Button_");

        [MenuItem("MIE/Change Prefix/Text_", false, 3)]
        private static void SetTextPrefix() => ApplyPrefix("Text_");

        [MenuItem("MIE/Change Prefix/Scroll_", false, 4)]
        private static void SetScrollPrefix() => ApplyPrefix("Scroll_");

        [MenuItem("MIE/Change Prefix Window")]
        public static void ShowWindow()
        {
            PrefixWindow window = GetWindow<PrefixWindow>("Prefix Changer");
            window.Show();
        }

        public class PrefixWindow : EditorWindow
        {
            private Vector2 scrollPos;

            private void OnGUI()
            {
                GUILayout.Label("Selected Objects Prefix Changer", EditorStyles.boldLabel);
                GUILayout.Space(5);

                if (Selection.gameObjects.Length == 0)
                {
                    EditorGUILayout.HelpBox("선택된 오브젝트가 없습니다.", MessageType.Info);
                    return;
                }

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                foreach (var prefix in prefixes)
                {
                    if (GUILayout.Button(prefix))
                    {
                        ApplyPrefix(prefix);
                    }
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private static void ApplyPrefix(string prefix)
        {
            if (Selection.gameObjects.Length == 0) return;

            foreach (var go in Selection.gameObjects)
            {
                Undo.RecordObject(go, "Change Prefix");
                string nameWithoutOldPrefix = RemoveExistingPrefix(go.name);
                go.name = prefix + nameWithoutOldPrefix;
                EditorUtility.SetDirty(go);
            }
        }

        private static string RemoveExistingPrefix(string name)
        {
            int index = name.IndexOf('_');
            if (index >= 0)
                return name.Substring(index + 1);
            return name;
        }
    }
}
