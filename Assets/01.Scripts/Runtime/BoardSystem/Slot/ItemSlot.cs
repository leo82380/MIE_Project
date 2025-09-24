using System.Collections.Generic;
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
            items.Push(item);
            item.SetLayer(0);
            foreach (var it in items)
            {
                it.AddLayer(1);
            }
        }

        /// <summary>
        /// 아이템 슬롯에 아이템을 생성하고 추가하는 메서드
        /// </summary>
        /// <returns>아이템 오브젝트</returns>
        public BaseItem PushItem()
        {
            var item = Instantiate(itemPrefab, itemParent);
            items.Push(item);
            item.SetLayer(0);
            foreach (var it in items)
            {
                it.AddLayer(1);
            }
            return item;
        }

        /// <summary>
        /// 아이템 슬롯에서 아이템을 제거하는 메서드
        /// </summary>
        /// <returns>제거된 아이템 오브젝트</returns>
        public BaseItem PopItem()
        {
            if (items.Count == 0) return null;
            var item = items.Pop();
            item.SetLayer(5);
            foreach (var it in items)
            {
                it.AddLayer(-1);
            }
            return item;
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
    }
}