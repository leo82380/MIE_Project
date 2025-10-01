using System;
using DG.Tweening;
using UnityEngine;

namespace MIE.UI.Component
{
    public class FadePanel : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 0.3f;
        protected CanvasGroup group;

        protected virtual void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        public void OpenPanel(Action onComplete = null)
        {
            EnablePanel(true, onComplete);
        }

        public void ClosePanel(Action onComplete = null)
        {
            EnablePanel(false, onComplete);
        }

        private void EnablePanel(bool enable, Action onComplete = null)
        {
            float targetAlpha = enable ? 1 : 0;
            group.DOFade(targetAlpha, fadeDuration).OnComplete(() =>
            {
                SetInteractable(enable);
                onComplete?.Invoke();
            });
        }

        private void SetInteractable(bool enable)
        {
            group.blocksRaycasts = enable;
            group.interactable = enable;
        }
    }
}