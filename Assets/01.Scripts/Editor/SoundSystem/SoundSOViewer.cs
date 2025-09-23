using MIE.Runtime.SoundSystem.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace MIE.Editor.SoundSystem
{
    public class SoundSOViewer : EditorWindow
    {
        private System.Collections.Generic.List<SoundSO> soundSOList = new System.Collections.Generic.List<SoundSO>();
        private int selectedIndex = -1;
        private Vector2 leftScroll, rightScroll;
        private const string SO_FOLDER = "Assets/06.SO/Sounds";

        [MenuItem("MIE/Tool/SoundSO Viewer")]
        public static void ShowWindow()
        {
            GetWindow<SoundSOViewer>("SoundSO Viewer");
        }

        private void OnEnable()
        {
            RefreshSOList();
        }

        private void RefreshSOList()
        {
            soundSOList = new System.Collections.Generic.List<SoundSO>();
            var guids = AssetDatabase.FindAssets("t:SoundSO", new[] { SO_FOLDER });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<SoundSO>(path);
                if (so != null) soundSOList.Add(so);
            }
            if (soundSOList.Count > 0 && (selectedIndex < 0 || selectedIndex >= soundSOList.Count))
                selectedIndex = 0;
        }

        private static int renameIndex = -1;
        private static string renameBuffer = null;

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("\u266B SoundSO Viewer", new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 20,
                normal = { textColor = new Color(0.45f, 0.7f, 1.0f) }
            }, GUILayout.Height(32));
            GUILayout.FlexibleSpace();
            GUI.backgroundColor = new Color(0.18f, 0.45f, 0.7f);
            if (GUILayout.Button("+ Create SoundSO", new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 13,
                fixedHeight = 32,
                normal = { textColor = Color.white }
            }, GUILayout.Width(160)))
            {
                CreateNewSoundSO();
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(6);
            EditorGUILayout.BeginHorizontal();

            leftScroll = EditorGUILayout.BeginScrollView(leftScroll, GUILayout.Width(210), GUILayout.ExpandHeight(true));
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.clickCount == 2 && e.button == 0)
            {
                Vector2 mouse = e.mousePosition;
                float y = 0;
                for (int i = 0; i < soundSOList.Count; i++)
                {
                    float btnHeight = 38;
                    Rect btnRect = new Rect(0, y, 210, btnHeight);
                    if (btnRect.Contains(mouse))
                    {
                        renameIndex = i;
                        renameBuffer = soundSOList[i] ? soundSOList[i].name : "";
                        GUI.FocusControl("SO_RENAME_FIELD");
                        e.Use();
                        break;
                    }
                    y += btnHeight;
                }
            }
            for (int i = 0; i < soundSOList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                Color cardColor = (i == selectedIndex) ? new Color(0.18f, 0.45f, 0.7f, 0.85f) : new Color(0.22f, 0.23f, 0.26f, 0.95f);
                EditorGUI.DrawRect(GUILayoutUtility.GetRect(180, 36, GUILayout.ExpandWidth(true)), cardColor);
                GUILayout.Space(-180);
                if (renameIndex == i)
                {
                    GUI.SetNextControlName("SO_RENAME_FIELD");
                    string newName = EditorGUILayout.TextField(renameBuffer, GUILayout.Width(120), GUILayout.Height(28));
                    if (newName != renameBuffer)
                    {
                        renameBuffer = newName;
                    }
                    if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
                    {
                        if (soundSOList[i] && !string.IsNullOrEmpty(renameBuffer) && renameBuffer != soundSOList[i].name)
                        {
                            string path = AssetDatabase.GetAssetPath(soundSOList[i]);
                            AssetDatabase.RenameAsset(path, renameBuffer);
                            AssetDatabase.SaveAssets();
                            RefreshSOList();
                        }
                        renameIndex = -1;
                        renameBuffer = null;
                        GUIUtility.keyboardControl = 0;
                        GUI.changed = true;
                        Repaint();
                    }
                }
                else
                {
                    if (GUILayout.Button(soundSOList[i] ? soundSOList[i].name : "(null)", new GUIStyle(EditorStyles.label)
                    {
                        fontSize = 14,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleLeft,
                        normal = { textColor = (i == selectedIndex) ? Color.white : new Color(0.7f, 0.8f, 1.0f) }
                    }, GUILayout.Width(140), GUILayout.Height(36)))
                    {
                        selectedIndex = i;
                    }
                }
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = new Color(0.8f, 0.2f, 0.2f);
                if (GUILayout.Button("\u2716", new GUIStyle(GUI.skin.button)
                {
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    fixedWidth = 28,
                    fixedHeight = 28,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Color.white }
                }, GUILayout.Width(28), GUILayout.Height(28)))
                {
                    string path = AssetDatabase.GetAssetPath(soundSOList[i]);
                    if (!string.IsNullOrEmpty(path))
                    {
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                        RefreshSOList();
                        if (selectedIndex >= soundSOList.Count) selectedIndex = soundSOList.Count - 1;
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(2);
                        continue;
                    }
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(2);
            }

            EditorGUILayout.EndScrollView();

            GUILayout.Space(2);

            EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(8), GUILayout.ExpandHeight(true));

            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            GUILayout.Space(8);
            EditorGUILayout.LabelField("SoundSO Inspector", new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                normal = { textColor = new Color(0.45f, 0.7f, 1.0f) }
            });
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            rightScroll = EditorGUILayout.BeginScrollView(rightScroll, GUILayout.ExpandHeight(true));
            if (selectedIndex >= 0 && selectedIndex < soundSOList.Count && soundSOList[selectedIndex] != null)
            {
                EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(16, 16, 16, 16),
                    margin = new RectOffset(8, 8, 8, 8),
                    normal = { background = Texture2D.blackTexture }
                });
                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(soundSOList[selectedIndex]);
                if (editor != null)
                {
                    editor.OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("\u2190 왼쪽에서 SoundSO를 선택하세요", new GUIStyle(EditorStyles.label)
                {
                    fontSize = 13,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = new Color(0.6f, 0.7f, 0.8f) }
                }, GUILayout.ExpandHeight(true));
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void CreateNewSoundSO()
        {
            string folder = SO_FOLDER;
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/SoundSO.asset");
            var so = ScriptableObject.CreateInstance<SoundSO>();
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshSOList();
            selectedIndex = soundSOList.FindIndex(x => AssetDatabase.GetAssetPath(x) == path);
        }
    }
}
