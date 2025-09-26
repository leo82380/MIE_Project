using UnityEngine;
using UnityEditor;

namespace MIE.Editor.Component
{
    [InitializeOnLoad]
    public static class CanvasGroupHierarchyToggle
    {
        static CanvasGroupHierarchyToggle()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null) return;

            CanvasGroup cg = go.GetComponent<CanvasGroup>();
            if (cg == null) return;

            Rect buttonRect = new Rect(selectionRect.xMax - 70, selectionRect.y, 45, selectionRect.height);

            bool isOn = (cg.alpha > 0.5f && cg.interactable && cg.blocksRaycasts);

            string buttonText = isOn ? "Off" : "On";

            if (GUI.Button(buttonRect, buttonText))
            {
                Undo.RecordObject(cg, "Toggle CanvasGroup");

                bool turnOn = !isOn;
                cg.alpha = turnOn ? 1f : 0f;
                cg.interactable = turnOn;
                cg.blocksRaycasts = turnOn;

                EditorUtility.SetDirty(cg);
            }
        }
    }
}
