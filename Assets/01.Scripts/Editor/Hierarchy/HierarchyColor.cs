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

            DrawAlternatingBackground(selectionRect, instanceID);

            foreach (var kvp in config.ColorMap)
            {
                if (obj.name.Contains(kvp.Key))
                {
                    DrawEnhancedRect(selectionRect, kvp.Value, obj);
                    break;
                }
            }

            DrawHierarchyDecorations(selectionRect, obj);
            DrawObjectStatusIndicator(selectionRect, obj);
            DrawComponentPreview(selectionRect, obj);
        }

        private static Texture2D gradientTexture;
        
        private static void DrawGradientRect(Rect rect, Color baseColor)
        {
            // 텍스처 기반 그라데이션으로 완벽한 연속성 보장
            if (gradientTexture == null || gradientTexture.width != (int)rect.width)
            {
                CreateGradientTexture((int)rect.width);
            }
            
            // 원본 색상에 그라데이션 적용
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

        // 스트라이프 배경 효과
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
            // 둥근 배경 카드 효과
            DrawRoundedCard(rect, baseColor);
            
            // 동그란 왼쪽 인디케이터
            DrawRoundLeftIndicator(rect, baseColor);
            
            // 동그란 우측 장식들
            DrawRoundDecorations(rect, baseColor);
        }

        // 둥근 카드 배경
        private static void DrawRoundedCard(Rect rect, Color baseColor)
        {
            // 메인 카드 영역 (약간 안쪽으로)
            Rect cardRect = new Rect(rect.x + 2, rect.y + 1, rect.width - 4, rect.height - 2);
            
            // 기본 그라데이션 배경
            DrawGradientRect(cardRect, baseColor);
            
            // 둥근 모서리 효과 (코너 마스킹)
            DrawRoundedCorners(cardRect, baseColor);
        }

        private static void DrawRoundLeftIndicator(Rect rect, Color baseColor)
        {
            float barWidth = 2f;
            Rect barRect = new Rect(rect.x + 18, rect.y + 2, barWidth, rect.height - 4);
            
            Color softColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.4f);
            EditorGUI.DrawRect(barRect, softColor);
            
            Rect highlightRect = new Rect(rect.x + 17, rect.y + rect.height * 0.2f, 1, rect.height * 0.6f);
            Color highlightColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.6f);
            EditorGUI.DrawRect(highlightRect, highlightColor);
        }

        // 깔끔한 우측 장식
        private static void DrawRoundDecorations(Rect rect, Color baseColor)
        {
            // 오른쪽 가장자리에 미묘한 그라데이션만
            Rect fadeRect = new Rect(rect.x + rect.width - 10, rect.y + 1, 8, rect.height - 2);
            
            // 부드러운 페이드 아웃 효과
            for (int i = 0; i < 8; i++)
            {
                float alpha = Mathf.Lerp(0.1f, 0f, (float)i / 7f);
                Color fadeColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * alpha);
                Rect lineRect = new Rect(fadeRect.x + i, fadeRect.y, 1, fadeRect.height);
                EditorGUI.DrawRect(lineRect, fadeColor);
            }
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

        // 동그란 원 그리기
        private static void DrawCircle(Rect rect, Color color)
        {
            float radius = Mathf.Min(rect.width, rect.height) / 2f;
            Vector2 center = new Vector2(rect.x + rect.width / 2, rect.y + rect.height / 2);
            
            // 원을 픽셀 단위로 그리기
            for (float x = rect.x; x < rect.x + rect.width; x++)
            {
                for (float y = rect.y; y < rect.y + rect.height; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    if (distance <= radius)
                    {
                        float alpha = Mathf.Lerp(1f, 0.3f, distance / radius);
                        Color pixelColor = new Color(color.r, color.g, color.b, color.a * alpha);
                        EditorGUI.DrawRect(new Rect(x, y, 1, 1), pixelColor);
                    }
                }
            }
        }

        private static void DrawHierarchyDecorations(Rect rect, GameObject obj)
        {
            if (obj.layer != 0)
            {
                Rect layerRect = new Rect(rect.x + rect.width - 95, rect.y + 3, 25, rect.height - 6);
                
                Color layerColor = GetLayerColor(obj.layer);
                EditorGUI.DrawRect(layerRect, layerColor);
                
                GUIStyle layerStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    fontSize = 7,
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };
                layerStyle.normal.textColor = Color.white;
                
                // 레이어 이름 표시 (짧게)
                string layerName = GetShortLayerName(obj.layer);
                GUI.Label(layerRect, layerName, layerStyle);
            }
        }

        private static void DrawObjectStatusIndicator(Rect rect, GameObject obj)
        {
            float dotSize = 8f;
            Rect statusRect = new Rect(rect.x + 8, rect.y + (rect.height - dotSize) / 2, dotSize, dotSize);
            
            if (!obj.activeInHierarchy)
            {
                DrawCircle(statusRect, new Color(1f, 0.3f, 0.3f, 0.9f));
                DrawSmallX(statusRect, Color.white);
            }
            else if (PrefabUtility.GetPrefabInstanceStatus(obj) != PrefabInstanceStatus.NotAPrefab)
            {
                DrawCircle(statusRect, new Color(0.3f, 0.7f, 1f, 0.9f));
                DrawSmallP(statusRect, Color.white);
            }
        }

        private static void DrawComponentPreview(Rect rect, GameObject obj)
        {
            if (!obj.tag.Equals("Untagged"))
            {
                Rect tagRect = new Rect(rect.x + rect.width - 120, rect.y + 3, 20, rect.height - 6);
                
                Color tagColor = GetTagColor(obj.tag);
                EditorGUI.DrawRect(tagRect, tagColor);
                
                GUIStyle tagStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    fontSize = 8,
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };
                tagStyle.normal.textColor = Color.white;
                GUI.Label(tagRect, GetShortTag(obj.tag), tagStyle);
            }
        }

        // 작은 X 표시 (비활성화용)
        private static void DrawSmallX(Rect rect, Color color)
        {
            float centerX = rect.x + rect.width / 2;
            float centerY = rect.y + rect.height / 2;
            float size = rect.width * 0.3f;
            
            // X 모양의 작은 선들
            for (float i = -size; i <= size; i++)
            {
                EditorGUI.DrawRect(new Rect(centerX + i - 0.5f, centerY + i - 0.5f, 1, 1), color);
                EditorGUI.DrawRect(new Rect(centerX + i - 0.5f, centerY - i - 0.5f, 1, 1), color);
            }
        }

        // 작은 P 표시 (프리팹용)
        private static void DrawSmallP(Rect rect, Color color)
        {
            float centerX = rect.x + rect.width / 2 - 1;
            float centerY = rect.y + rect.height / 2;
            
            // P 모양 (간단한 버전)
            EditorGUI.DrawRect(new Rect(centerX - 1, centerY - 2, 1, 4), color); // 세로선
            EditorGUI.DrawRect(new Rect(centerX, centerY - 2, 2, 1), color);     // 위 가로선
            EditorGUI.DrawRect(new Rect(centerX, centerY - 1, 2, 1), color);     // 중간 가로선
            EditorGUI.DrawRect(new Rect(centerX + 1, centerY - 1, 1, 1), color); // 오른쪽 연결
        }

        // 점선 효과
        private static void DrawDottedLine(Rect rect, Color color)
        {
            for (float y = rect.y; y < rect.y + rect.height; y += 3)
            {
                Rect dot = new Rect(rect.x, y, 1, 1);
                EditorGUI.DrawRect(dot, color);
            }
        }

        // 둥근 배지 그리기
        private static void DrawRoundedBadge(Rect rect, Color color)
        {
            // 메인 배지 배경
            EditorGUI.DrawRect(rect, color);
            
            // 상단 하이라이트
            Rect topHighlight = new Rect(rect.x, rect.y, rect.width, 1);
            EditorGUI.DrawRect(topHighlight, new Color(1f, 1f, 1f, 0.3f));
            
            // 하단 그림자
            Rect bottomShadow = new Rect(rect.x, rect.y + rect.height - 1, rect.width, 1);
            EditorGUI.DrawRect(bottomShadow, new Color(0f, 0f, 0f, 0.3f));
            
            // 둥근 모서리 효과
            DrawBadgeCorners(rect, color);
        }

        // 그라데이션 배지 그리기
        private static void DrawGradientBadge(Rect rect, Color color)
        {
            // 세로 그라데이션 효과
            int segments = (int)rect.height;
            for (int i = 0; i < segments; i++)
            {
                float t = (float)i / (segments - 1);
                float brightness = Mathf.Lerp(1.2f, 0.8f, t);
                Color segmentColor = new Color(
                    color.r * brightness, 
                    color.g * brightness, 
                    color.b * brightness, 
                    color.a
                );
                
                Rect segment = new Rect(rect.x, rect.y + i, rect.width, 1);
                EditorGUI.DrawRect(segment, segmentColor);
            }
            
            // 테두리
            DrawBadgeBorder(rect, new Color(0f, 0f, 0f, 0.4f));
        }

        // 배지 모서리 처리
        private static void DrawBadgeCorners(Rect rect, Color color)
        {
            // 간단한 모서리 둥글게 처리
            Color cornerColor = new Color(color.r, color.g, color.b, color.a * 0.5f);
            
            // 모서리 픽셀들
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1, 1), cornerColor);
            EditorGUI.DrawRect(new Rect(rect.x + rect.width - 1, rect.y, 1, 1), cornerColor);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height - 1, 1, 1), cornerColor);
            EditorGUI.DrawRect(new Rect(rect.x + rect.width - 1, rect.y + rect.height - 1, 1, 1), cornerColor);
        }

        // 배지 테두리 그리기
        private static void DrawBadgeBorder(Rect rect, Color color)
        {
            // 상하좌우 테두리
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height - 1, rect.width, 1), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.x + rect.width - 1, rect.y, 1, rect.height), color);
        }

        // 레이어 아이콘
        private static string GetLayerIcon(int layer)
        {
            switch (layer)
            {
                case 1: return "🎨"; // UI
                case 2: return "🌍"; // Ground  
                case 3: return "👤"; // Player
                case 8: return "👹"; // Enemy
                case 9: return "💎"; // Collectible
                default: return "📋"; // Other
            }
        }

        // 태그 아이콘
        private static string GetTagIcon(string tag)
        {
            switch (tag.ToLower())
            {
                case "player": return "👤";
                case "enemy": return "👹";
                case "ui": return "🎨";
                case "ground": return "🌍";
                case "collectible": return "💎";
                case "respawn": return "🔄";
                case "finish": return "🏁";
                default: return "🏷️";
            }
        }

        // 레이어 이름 줄이기
        private static string GetShortLayerName(int layer)
        {
            string layerName = LayerMask.LayerToName(layer);
            
            if (string.IsNullOrEmpty(layerName))
            {
                return layer.ToString(); // 이름이 없으면 숫자로
            }
            
            // 일반적인 레이어 이름 줄이기
            switch (layerName.ToLower())
            {
                case "ui": return "UI";
                case "player": return "P";
                case "enemy": return "E";
                case "ground": return "G";
                case "water": return "W";
                case "ignore raycast": return "IGN";
                case "transparentfx": return "FX";
                default:
                    // 3글자로 줄이기
                    return layerName.Length <= 3 ? layerName.ToUpper() : layerName.Substring(0, 3).ToUpper();
            }
        }

        // 깔끔한 레이어 색상
        private static Color GetLayerColor(int layer)
        {
            switch (layer)
            {
                case 1: return new Color(0.2f, 0.5f, 0.8f, 0.7f);      // TransparentFX - 블루
                case 2: return new Color(0.4f, 0.7f, 0.2f, 0.7f);      // Ignore Raycast - 그린
                case 3: return new Color(0.8f, 0.3f, 0.3f, 0.7f);      // Water - 레드
                case 4: return new Color(0.2f, 0.4f, 0.8f, 0.7f);      // UI - 인디고
                case 8: return new Color(0.7f, 0.4f, 0.1f, 0.7f);      // Custom - 오렌지
                case 9: return new Color(0.6f, 0.2f, 0.8f, 0.7f);      // Custom - 퍼플
                default: return new Color(0.5f, 0.5f, 0.5f, 0.6f);     // Other - 그레이
            }
        }

        // 깔끔한 태그 색상
        private static Color GetTagColor(string tag)
        {
            switch (tag.ToLower())
            {
                case "player": return new Color(0.2f, 0.7f, 0.3f, 0.8f);
                case "enemy": return new Color(0.8f, 0.2f, 0.2f, 0.8f);
                case "ui": return new Color(0.2f, 0.4f, 0.8f, 0.8f);
                case "ground": return new Color(0.6f, 0.4f, 0.2f, 0.8f);
                case "collectible": return new Color(0.9f, 0.6f, 0.1f, 0.8f);
                case "respawn": return new Color(0.7f, 0.2f, 0.6f, 0.8f);
                case "finish": return new Color(0.1f, 0.6f, 0.7f, 0.8f);
                default: return new Color(0.5f, 0.5f, 0.5f, 0.7f);
            }
        }

        // 태그 이름 줄이기 (더 직관적으로)
        private static string GetShortTag(string tag)
        {
            if (tag.Length <= 2) return tag.ToUpper();
            
            switch (tag.ToLower())
            {
                case "player": return "P";
                case "enemy": return "E";
                case "ground": return "G";
                case "collectible": return "C";
                case "respawn": return "R";
                case "finish": return "F";
                case "maincamera": return "📷";
                case "ui": return "UI";
                default: return tag.Substring(0, 1).ToUpper();
            }
        }

        // 컴포넌트 타입별 색상
        private static Color GetComponentColor(UnityEngine.Component component)
        {
            switch (component)
            {
                case Renderer _: return Color.green;
                case Collider _: return Color.yellow;
                case Rigidbody _: return Color.red;
                case AudioSource _: return Color.magenta;
                case Light _: return Color.white;
                case Camera _: return Color.cyan;
                default: return Color.gray;
            }
        }
    }
}