using System;
using System.Collections.Generic;
using System.Linq;
using MIE.BoardSystem.Item;
using MIE.BoardSystem.Item.Data;
using UnityEngine;

namespace MIE.BoardSystem.Slot
{
    // 아이템 슬롯(일반 슬롯의 아이템을 넣을 수 있는 칸 슬롯)
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private BaseItem itemPrefab;
        [SerializeField] private RectTransform itemParent;
        private Stack<BaseItem> items;

        public event Action<Stack<BaseItem>> OnItemChanged; // 아이템이 추가되거나 제거될 때 발생하는 이벤트

        private void Awake()
        {
            items = new Stack<BaseItem>();
        }

        /// <summary>
        /// 아이템 슬롯에 아이템을 추가하는 메서드
        /// </summary>
        /// <param name="item">드롭한 아이템</param>
        public void PushItem(BaseItem item)
        {
            // 새 아이템을 먼저 스택에 추가하고 부모 설정
            items.Push(item);
            item.transform.SetParent(itemParent);

            RefreshAllLayers();
            OnItemChanged?.Invoke(items);
        }

        /// <summary>
        /// 아이템 슬롯에 아이템을 생성하고 추가하는 메서드
        /// </summary>
        /// <returns>아이템 오브젝트</returns>
        public BaseItem PushItem()
        {
            // 새 아이템을 생성하고 스택에 추가
            var item = Instantiate(itemPrefab, itemParent);
            items.Push(item);
            
            // 스택의 모든 아이템 레이어를 다시 설정
            RefreshAllLayers();
            OnItemChanged?.Invoke(items);
            return item;
        }

        /// <summary>
        /// 아이템 슬롯에서 아이템을 제거하는 메서드 (0번 레이어만 제거 가능)
        /// </summary>
        /// <returns>제거된 아이템 오브젝트</returns>
        public BaseItem PopItem()
        {
            if (items.Count == 0) return null;
            
            var item = items.Pop();

            item.SetLayer(5);
            return item;
        }
        
        /// <summary>
        /// 스택에 있는 모든 아이템의 레이어를 올바르게 재설정
        /// </summary>
        private void RefreshAllLayers()
        {
            var itemArray = items.ToArray();
            for (int i = 0; i < itemArray.Length; i++)
            {
                itemArray[i].SetLayer(i);
            }
        }

        /// <summary>
        /// 아이템 슬롯의 최상단 아이템을 반환하는 메서드
        /// </summary>
        public ItemDataSO GetFront()
        {
            if (items.Count == 0) return null;
            return items.Peek().GetItemData();
        }

        /// <summary>
        /// 아이템 슬롯의 최상단 아이템의 레이어를 반환하는 메서
        /// </summary>
        /// <returns>최상단 아이템의 레이어</returns>
        public int GetFrontLayer()
        {
            if (items.Count == 0) return -1;
            return items.Peek().Layer;
        }

        /// <summary>
        /// 0번 레이어 아이템이 있는지 확인
        /// </summary>
        public bool IsContainsLayerZero()
        {
            foreach (var item in items)
            {
                if (item.Layer == 0) return true;
            }
            return false;
        }
        
        /// <summary>
        /// 드래그 가능한 아이템인지 확인 (0번 레이어만 드래그 가능)
        /// </summary>
        public bool CanDrag()
        {
            return items.Count > 0 && items.Peek().Layer == 0;
        }
        
        /// <summary>
        /// 현재 0번 레이어 아이템을 반환 (드래그용)
        /// </summary>
        public BaseItem GetDraggableItem()
        {
            if (CanDrag())
            {
                return items.Peek();
            }
            return null;
        }
        
        /// <summary>
        /// 아이템 슬롯이 비어있는지 확인
        /// </summary>
        public bool IsEmpty()
        {
            return items.Count == 0;
        }
        
        /// <summary>
        /// 스택에서 특정 아이템을 제거 (드래그 시작용)
        /// </summary>
        public void RemoveItemFromStack(BaseItem item)
        {
            if (items.Count == 0) return;
            
            // 맨 위 아이템(0번 레이어)만 제거 가능
            if (items.Peek() == item)
            {
                items.Pop();
                OnItemChanged?.Invoke(items);
            }
        }
        
        /// <summary>
        /// 스택의 모든 아이템 레이어를 1씩 감소 (BaseSlot에서 호출)
        /// </summary>
        public void ReduceAllLayers()
        {
            var itemArray = items.ToArray();
            for (int i = 0; i < itemArray.Length; i++)
            {
                var currentLayer = itemArray[i].Layer;
                if (currentLayer > 0) // 0번 레이어는 더 이상 감소하지 않음
                {
                    itemArray[i].SetLayer(currentLayer - 1);
                }
            }
        }
    }
}