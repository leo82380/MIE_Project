using UnityEngine;

namespace MIE.Tool
{
    public static class DebugExtension
    {
        public static void LogColor(string message, Color color)
        {
            string hexColor = ColorUtility.ToHtmlStringRGBA(color);
            Debug.Log($"<color=#{hexColor}>{message}</color>");
        }
    }
}