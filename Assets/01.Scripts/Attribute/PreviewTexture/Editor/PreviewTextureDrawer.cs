using UnityEditor;
using UnityEngine;

namespace MIE.Attribute.PreviewTexture.Editor
{
    [CustomPropertyDrawer(typeof(PreviewTextureAttribute))]
    public class PreviewTextureDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var previewAttr = (PreviewTextureAttribute)attribute;
            float previewSize = previewAttr.size;

            // Texture 또는 Sprite
            Texture tex = null;
            if (property.objectReferenceValue is Texture t)
            {
                tex = t;
            }
            else if (property.objectReferenceValue is Sprite s)
            {
                tex = s.texture;
            }

            if (tex == null)
            {
                // 텍스처/스프라이트 없으면 기본 필드
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            // 프리뷰 영역 (왼쪽)
            Rect previewRect = new Rect(position.x, position.y, previewSize, previewSize);

            // 오브젝트 필드 영역 (오른쪽 하단)
            float fieldHeight = EditorGUIUtility.singleLineHeight;
            Rect fieldRect = new Rect(
                previewRect.xMax + 4,                                  // 이미지 오른쪽에 붙게
                previewRect.yMax - fieldHeight,                        // 이미지 하단 정렬
                position.width - previewSize - 4,                      // 남은 가로 공간
                fieldHeight
            );

            // 텍스처 미리보기
            EditorGUI.DrawPreviewTexture(previewRect, tex, null, ScaleMode.ScaleToFit);

            // 인풋 필드 (라벨 포함)
            EditorGUI.PropertyField(fieldRect, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var previewAttr = (PreviewTextureAttribute)attribute;

            // Sprite도 체크
            bool hasTexture = property.objectReferenceValue is Texture ||
                              property.objectReferenceValue is Sprite;

            if (!hasTexture)
                return EditorGUIUtility.singleLineHeight;

            return Mathf.Max(EditorGUIUtility.singleLineHeight, previewAttr.size);
        }
    }
}
