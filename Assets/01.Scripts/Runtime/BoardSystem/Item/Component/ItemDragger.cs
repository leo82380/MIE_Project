using System;
using System.Collections.Generic;
using MIE.BoardSystem.Slot;
using MIE.Manager.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MIE.BoardSystem.Item.Component
{
    public class ItemDragger : MonoBehaviour, IItemComponent, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializable
    {
        private RectTransform rectTransform;
        private RectTransform defaultParent;
        private Canvas mainCanvas;
        private CanvasGroup canvasGroup;

        private BaseItem baseItem;

        public event Action OnBeginDragEvent;
        public event Action OnEndDragEvent;

        public void Initialize()
        {
            rectTransform = transform as RectTransform;
            defaultParent = rectTransform.parent as RectTransform;
            mainCanvas = transform.root.GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            baseItem = GetComponent<BaseItem>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SetDraggable(false);
            rectTransform.SetParent(mainCanvas.transform);
            OnBeginDragEvent?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetDraggable(true);
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            OnEndDragEvent?.Invoke();

            Transform targetParent = null;
            foreach (var result in raycastResults)
            {
                // 아이템 슬롯에 직접 드래그했을 때
                if (result.gameObject.TryGetComponent<ItemSlot>(out var itemSlot))
                {
                    if (itemSlot.IsContainsLayerZero()) continue;
                    itemSlot.PushItem(baseItem);
                    targetParent = itemSlot.transform;
                    break;
                }
                // 일반 슬롯에 드래그했을 때
                if (result.gameObject.TryGetComponent<BaseSlot>(out var baseSlot))
                {
                    if (baseSlot.IsFull()) continue;
                    baseSlot.PushItem(baseItem);
                    targetParent = baseSlot.transform;
                    break;
                }
            }

            SetParent(targetParent ?? defaultParent);
        }

        public void SetParent(Transform parent)
        {
            rectTransform.SetParent(parent);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        public void RegisterEvent(IItem item)
        {
            item.OnEnableItem += HandleDraggable;
        }

        public void UnregisterEvent(IItem item)
        {
            item.OnEnableItem -= HandleDraggable;
        }

        private void HandleDraggable(int layer)
        {
            SetDraggable(layer == 0);
        }

        private void SetDraggable(bool isDraggable)
        {
            canvasGroup.blocksRaycasts = isDraggable;
            canvasGroup.interactable = isDraggable;
            canvasGroup.alpha = isDraggable ? 1f : 0.6f;
        }
    }
}