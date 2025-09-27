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
        private ItemSlot originalSlot; // 드래그 시작한 원래 슬롯

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
            
            // 원래 슬롯 기억하고 해당 슬롯에서 아이템 제거
            originalSlot = rectTransform.GetComponentInParent<ItemSlot>();
            if (originalSlot != null)
            {
                originalSlot.RemoveItemFromStack(baseItem);
            }
            
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

            Transform targetParent = null;
            bool droppedSuccessfully = false;
            
            foreach (var result in raycastResults)
            {
                // 아이템 슬롯에 직접 드래그했을 때
                if (result.gameObject.TryGetComponent<ItemSlot>(out var itemSlot))
                {
                    // 아이템 슬롯은 항상 아이템을 받을 수 있음 (스택 구조)
                    itemSlot.PushItem(baseItem);
                    targetParent = itemSlot.transform;
                    droppedSuccessfully = true;
                    break;
                }
                // 일반 슬롯에 드래그했을 때
                if (result.gameObject.TryGetComponent<BaseSlot>(out var baseSlot))
                {
                    if (baseSlot.IsFull()) continue; // 꽉 찬 슬롯은 건너뛰기
                    baseSlot.PushItem(baseItem, out targetParent);
                    droppedSuccessfully = true;
                    break;
                }
            }

            // 드롭에 실패했으면 원래 슬롯으로 되돌리기
            if (!droppedSuccessfully && originalSlot != null)
            {
                originalSlot.PushItem(baseItem);
                targetParent = originalSlot.transform;
            }

            // 원래 슬롯에서 0번 레이어가 모두 제거되었는지 확인하고 레이어 재정렬
            if (originalSlot != null)
            {
                var originalBaseSlot = originalSlot.GetComponentInParent<BaseSlot>();
                originalBaseSlot?.CheckAndRefreshAllLayers();
            }

            SetParent(targetParent ?? defaultParent);
            OnEndDragEvent?.Invoke();
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