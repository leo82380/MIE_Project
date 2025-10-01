using UnityEditor;
using UnityEngine;

namespace MIE.Editor.Hierarchy
{
    [InitializeOnLoad]
    public static class HierarchyColor
    {
        private static HierarchyColorConfig config;
        private const string configPath = "Assets/01.Scripts/Editor/Hierarchy/HierarchyColorConfig.asset";

        static HierarchyColor()
        {
            LoadOrCreateConfig();
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void LoadOrCreateConfig()
        {
            config = AssetDatabase.LoadAssetAtPath<HierarchyColorConfig>(configPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<HierarchyColorConfig>();
                AssetDatabase.CreateAsset(config, configPath);
                AssetDatabase.SaveAssets();
            }
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null || config == null) return;

            DrawAlternatingBackground(selectionRect, instanceID);

            foreach (var kvp in config.ColorMap)
            {
                if (obj.name.Contains(kvp.Key))
                {
                    DrawEnhancedRect(selectionRect, kvp.Value, obj);
                    break;
                }
            }
        }

        private static Texture2D gradientTexture;
        
        private static void DrawGradientRect(Rect rect, Color baseColor)
        {
            if (gradientTexture == null || gradientTexture.width != (int)rect.width)
            {
                CreateGradientTexture((int)rect.width);
            }
            
            Color originalColor = GUI.color;
            GUI.color = baseColor;
            
            GUI.DrawTexture(rect, gradientTexture, ScaleMode.StretchToFill);
            
            GUI.color = originalColor;
        }
        
        private static void CreateGradientTexture(int width)
        {
            if (gradientTexture != null)
            {
                Object.DestroyImmediate(gradientTexture);
            }
            
            gradientTexture = new Texture2D(width, 1);
            Color[] pixels = new Color[width];
            
            for (int x = 0; x < width; x++)
            {
                float normalizedX = width > 1 ? (float)x / (width - 1) : 0f;
                float alpha = Mathf.Lerp(0.2f, 1.0f, normalizedX);
                pixels[x] = new Color(1f, 1f, 1f, alpha);
            }
            
            gradientTexture.SetPixels(pixels);
            gradientTexture.Apply();
        }

        private static void DrawAlternatingBackground(Rect rect, int instanceID)
        {
            if (instanceID % 2 == 0)
            {
                EditorGUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.05f));
            }
        }

        // 동글동글한 스타일
        private static void DrawEnhancedRect(Rect rect, Color baseColor, GameObject obj)
        {
            DrawRoundedCard(rect, baseColor);
        }
        
        private static void DrawRoundedCard(Rect rect, Color baseColor)
        {
            Rect cardRect = new Rect(rect.x + 2, rect.y + 1, rect.width - 4, rect.height - 2);
            DrawGradientRect(cardRect, baseColor);
            DrawRoundedCorners(cardRect, baseColor);
        }

        // 둥근 모서리 효과
        private static void DrawRoundedCorners(Rect rect, Color baseColor)
        {
            float cornerRadius = 3f;
            
            // 코너 부분을 부드럽게 처리
            // 왼쪽 위 코너
            DrawCornerPixels(rect.x, rect.y, cornerRadius, baseColor, true, true);
            // 오른쪽 위 코너  
            DrawCornerPixels(rect.x + rect.width, rect.y, cornerRadius, baseColor, false, true);
            // 왼쪽 아래 코너
            DrawCornerPixels(rect.x, rect.y + rect.height, cornerRadius, baseColor, true, false);
            // 오른쪽 아래 코너
            DrawCornerPixels(rect.x + rect.width, rect.y + rect.height, cornerRadius, baseColor, false, false);
        }

        // 코너 픽셀 그리기
        private static void DrawCornerPixels(float centerX, float centerY, float radius, Color color, bool isLeft, bool isTop)
        {
            for (float x = -radius; x <= radius; x++)
            {
                for (float y = -radius; y <= radius; y++)
                {
                    float distance = Mathf.Sqrt(x * x + y * y);
                    if (distance <= radius)
                    {
                        float alpha = Mathf.Lerp(0.8f, 0.2f, distance / radius);
                        Color pixelColor = new Color(color.r, color.g, color.b, color.a * alpha);
                        
                        float pixelX = centerX + (isLeft ? x : -x);
                        float pixelY = centerY + (isTop ? y : -y);
                        
                        Rect pixelRect = new Rect(pixelX, pixelY, 1, 1);
                        EditorGUI.DrawRect(pixelRect, pixelColor);
                    }
                }
            }
        }
    }
}