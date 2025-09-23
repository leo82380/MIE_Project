using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor.Common
{
    public abstract class SOViewer<T> : EditorWindow where T : ScriptableObject
    {
        protected List<T> soList = new List<T>();
        protected int selectedIndex = -1;
        protected Vector2 leftScroll, rightScroll;
        
        protected abstract string SOFolderPath { get; }
        protected abstract string WindowTitle { get; }
        protected abstract string InspectorTitle { get; }
        protected abstract string CreateButtonText { get; }
        protected abstract string SelectionHint { get; }
        protected abstract string DefaultAssetName { get; }

        protected virtual void OnEnable()
        {
            RefreshSOList();
        }

        protected virtual void RefreshSOList()
        {
            soList = new List<T>();
            var typeName = typeof(T).Name;
            var guids = AssetDatabase.FindAssets($"t:{typeName}", new[] { SOFolderPath });
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<T>(path);
                if (so != null) soList.Add(so);
            }
            
            if (soList.Count > 0 && (selectedIndex < 0 || selectedIndex >= soList.Count))
                selectedIndex = 0;
        }

        private static int renameIndex = -1;
        private static string renameBuffer = null;

        protected virtual void OnGUI()
        {
            DrawHeader();
            EditorGUILayout.Space(6);
            EditorGUILayout.BeginHorizontal();
            DrawLeftPanel();
            DrawSeparator();
            DrawRightPanel();
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"\u266B {WindowTitle}", new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 20,
                normal = { textColor = new Color(0.45f, 0.7f, 1.0f) }
            }, GUILayout.Height(32));
            
            GUILayout.FlexibleSpace();
            GUI.backgroundColor = new Color(0.18f, 0.45f, 0.7f);
            
            if (GUILayout.Button($"+ {CreateButtonText}", new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 13,
                fixedHeight = 32,
                normal = { textColor = Color.white }
            }, GUILayout.Width(160)))
            {
                CreateNewSO();
            }
            
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawLeftPanel()
        {
            leftScroll = EditorGUILayout.BeginScrollView(leftScroll, GUILayout.Width(210), GUILayout.ExpandHeight(true));
            
            HandleDoubleClickRename();
            DrawSOList();
            
            EditorGUILayout.EndScrollView();
        }

        protected virtual void HandleDoubleClickRename()
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.clickCount == 2 && e.button == 0)
            {
                Vector2 mouse = e.mousePosition;
                float y = 0;
                for (int i = 0; i < soList.Count; i++)
                {
                    float btnHeight = 38;
                    Rect btnRect = new Rect(0, y, 210, btnHeight);
                    if (btnRect.Contains(mouse))
                    {
                        renameIndex = i;
                        renameBuffer = soList[i] ? soList[i].name : "";
                        GUI.FocusControl("SO_RENAME_FIELD");
                        e.Use();
                        break;
                    }
                    y += btnHeight;
                }
            }
        }

        protected virtual void DrawSOList()
        {
            for (int i = 0; i < soList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                
                Color cardColor = (i == selectedIndex) ? 
                    new Color(0.18f, 0.45f, 0.7f, 0.85f) : 
                    new Color(0.22f, 0.23f, 0.26f, 0.95f);
                    
                EditorGUI.DrawRect(GUILayoutUtility.GetRect(180, 36, GUILayout.ExpandWidth(true)), cardColor);
                GUILayout.Space(-180);

                if (renameIndex == i)
                {
                    DrawRenameField(i);
                }
                else
                {
                    DrawSOButton(i);
                }

                DrawDeleteButton(i);
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(2);
            }
        }

        protected virtual void DrawRenameField(int index)
        {
            GUI.SetNextControlName("SO_RENAME_FIELD");
            string newName = EditorGUILayout.TextField(renameBuffer, GUILayout.Width(120), GUILayout.Height(28));
            
            if (newName != renameBuffer)
            {
                renameBuffer = newName;
            }
            
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                if (soList[index] && !string.IsNullOrEmpty(renameBuffer) && renameBuffer != soList[index].name)
                {
                    string path = AssetDatabase.GetAssetPath(soList[index]);
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

        protected virtual void DrawSOButton(int index)
        {
            if (GUILayout.Button(soList[index] ? soList[index].name : "(null)", new GUIStyle(EditorStyles.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = (index == selectedIndex) ? Color.white : new Color(0.7f, 0.8f, 1.0f) }
            }, GUILayout.Width(140), GUILayout.Height(36)))
            {
                selectedIndex = index;
                OnSOSelected(soList[index]);
            }
        }

        protected virtual void DrawDeleteButton(int index)
        {
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
                DeleteSO(index);
            }
            
            GUI.backgroundColor = Color.white;
        }

        protected virtual void DrawSeparator()
        {
            GUILayout.Space(2);
            EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(8), GUILayout.ExpandHeight(true));
        }

        protected virtual void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            GUILayout.Space(8);
            
            EditorGUILayout.LabelField($"{InspectorTitle} Inspector", new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                normal = { textColor = new Color(0.45f, 0.7f, 1.0f) }
            });
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            rightScroll = EditorGUILayout.BeginScrollView(rightScroll, GUILayout.ExpandHeight(true));
            
            if (selectedIndex >= 0 && selectedIndex < soList.Count && soList[selectedIndex] != null)
            {
                DrawInspector();
            }
            else
            {
                DrawEmptySelection();
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawInspector()
        {
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(16, 16, 16, 16),
                margin = new RectOffset(8, 8, 8, 8),
                normal = { background = Texture2D.blackTexture }
            });
            
            UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(soList[selectedIndex]);
            if (editor != null)
            {
                editor.OnInspectorGUI();
            }
            
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawEmptySelection()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField($"\u2190 {SelectionHint}", new GUIStyle(EditorStyles.label)
            {
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.6f, 0.7f, 0.8f) }
            }, GUILayout.ExpandHeight(true));
            GUILayout.FlexibleSpace();
        }

        protected virtual void CreateNewSO()
        {
            string folder = SOFolderPath;
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
                
            string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{DefaultAssetName}.asset");
            var so = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshSOList();
            selectedIndex = soList.FindIndex(x => AssetDatabase.GetAssetPath(x) == path);
            
            OnSOCreated(so);
        }

        protected virtual void DeleteSO(int index)
        {
            string path = AssetDatabase.GetAssetPath(soList[index]);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                RefreshSOList();
                if (selectedIndex >= soList.Count) selectedIndex = soList.Count - 1;
                OnSODeleted();
            }
        }

        protected virtual void OnSOSelected(T selectedSO) { }
        protected virtual void OnSOCreated(T createdSO) { }
        protected virtual void OnSODeleted() { }
    }
}