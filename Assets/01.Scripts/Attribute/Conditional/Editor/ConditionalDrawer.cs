using UnityEditor;
using UnityEngine;

namespace MIE.Attribute.Conditional.Editor
{
    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalAttribute cond = (ConditionalAttribute)attribute;

            // 부모 오브젝트에서 조건 필드 찾기
            SerializedProperty boolProp = property.serializedObject.FindProperty(cond.BoolFieldName);
            bool show = boolProp != null && boolProp.boolValue;

            if (show)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalAttribute cond = (ConditionalAttribute)attribute;
            SerializedProperty boolProp = property.serializedObject.FindProperty(cond.BoolFieldName);

            bool show = boolProp != null && boolProp.boolValue;
            return show ? EditorGUI.GetPropertyHeight(property) : 0f;
        }
    }
}
