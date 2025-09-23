using UnityEngine;

namespace MIE.Editor
{
    public static class GUILayoutExtension
    {
        public static void DrawHorizontalLine(Color? color = null, float height = 1f)
        {
            Color originalColor = GUI.color;
            GUI.color = color ?? Color.gray; // 기본 색상은 회색

            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(height));

            GUI.color = originalColor;
        }
    }
}
