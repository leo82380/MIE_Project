using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor
{
    [CreateAssetMenu(menuName = "MIE/Settings/Hierarchy Color Config", fileName = "HierarchyColorConfig")]
    public class HierarchyColorConfig : ScriptableObject
    {
        [SerializedDictionary("Name", "Color")]
        public SerializedDictionary<string, Color> ColorMap = new SerializedDictionary<string, Color>();
    }

    [CustomEditor(typeof(HierarchyColorConfig))]
    public class HierarchyColorConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ColorMap"), true);

            serializedObject.ApplyModifiedProperties();

        }
    }
}
