using System.IO;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

namespace MIE.Editor.Hierarchy
{
    [InitializeOnLoad]
    public static class HierarchyColor
    {
        private static MIE.Editor.HierarchyColorConfig config;
        private const string configPath = "Assets/01.Scripts/Editor/Hierarchy/HierarchyColorConfig.asset";

        static HierarchyColor()
        {
            LoadOrCreateConfig();
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void LoadOrCreateConfig()
        {
            config = AssetDatabase.LoadAssetAtPath<MIE.Editor.HierarchyColorConfig>(configPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<MIE.Editor.HierarchyColorConfig>();
                AssetDatabase.CreateAsset(config, configPath);
                AssetDatabase.SaveAssets();
            }
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null || config == null) return;

            foreach (var kvp in config.ColorMap)
            {
                if (obj.name.Contains(kvp.Key))
                {
                    EditorGUI.DrawRect(selectionRect, kvp.Value);
                    break;
                }
            }
        }
    }
}