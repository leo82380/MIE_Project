using UnityEngine;

namespace MIE.UI.PopupSystem
{
    public class PopupPanel : MonoBehaviour
    {
        [SerializeField] private PopupType popupType;
        protected CanvasGroup group;

        public PopupType PopupType => popupType;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        public void OpenPanel()
        {
            EnablePanel(true);
        }

        public void ClosePanel()
        {
            EnablePanel(false);
        }
        
        private void EnablePanel(bool enable)
        {
            group.alpha = enable ? 1 : 0;
            group.blocksRaycasts = enable;
            group.interactable = enable;
        }
    }
}