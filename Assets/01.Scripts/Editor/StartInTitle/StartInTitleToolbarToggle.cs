using UnityToolbarExtender;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor.StartInTitle
{
    [InitializeOnLoad]
    public static class StartInTitleToolbarToggle
    {
        static StartInTitleToolbarToggle()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Start In Title");
            bool newValue = GUILayout.Toggle(
                StartInTitleEditor.IsEnabled, GUIContent.none);
            if (newValue != StartInTitleEditor.IsEnabled)
            {
                StartInTitleEditor.Toggle();
            }
            GUILayout.EndHorizontal();
        }
    }
}
