using UnityEngine;
using UnityEditor;

namespace MIE.Editor.Component
{
    [InitializeOnLoad]
    public static class GameObjectHierarchyToggleGUI
    {
        static GameObjectHierarchyToggleGUI()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null) return;

            Rect toggleRect = new Rect(selectionRect.xMax - 20, selectionRect.y, 18, selectionRect.height);

            bool isActive = go.activeSelf;

            // GUI.Toggle 생성
            bool toggled = GUI.Toggle(toggleRect, isActive, GUIContent.none);

            if (toggled != isActive)
            {
                Undo.RecordObject(go, "Toggle GameObject Active");
                go.SetActive(toggled);
                EditorUtility.SetDirty(go);
            }
        }
    }
}
