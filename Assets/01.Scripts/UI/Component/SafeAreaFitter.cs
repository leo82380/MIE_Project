using UnityEngine;

namespace MIE.UI.Component
{
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform _panel;

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
        }

        private void Start()
        {
            ApplySafeArea();
        }

        private void OnRectTransformDimensionsChange()
        {
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;

            _panel.offsetMin = Vector2.zero;
            _panel.offsetMax = Vector2.zero;
        }
    }
}