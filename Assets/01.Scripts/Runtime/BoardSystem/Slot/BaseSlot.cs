using System.Collections.Generic;
using MIE.BoardSystem.Item;
using UnityEngine;

namespace MIE.BoardSystem.Slot
{
    // 기본 슬롯 클래스
    public class BaseSlot : MonoBehaviour
    {
        [SerializeField] private int itemSlotCount = 3;
        [SerializeField] private ItemSlot itemSlotPrefab;
        [SerializeField] private RectTransform itemSlotParent;

        private List<ItemSlot> itemSlots;

        private void Awake()
        {
            InitializeSlot();
        }

        public bool CheckMerge()
        {
            int count = 0;

            // 가지고 있는 아이템 슬롯을 전부 확인
            for (int i = 0; i < itemSlots.Count; i++)
            {
                // 이전 아이템 슬롯과 현재 아이템 슬롯의 아이템이 동일한지, 레이어도 동일한지 확인
                var frontItem = itemSlots[i];
                var beforeItem = i > 0 ? itemSlots[i - 1] : null;
                if (frontItem != null &&
                    frontItem.GetFront().ItemID == beforeItem?.GetFront().ItemID &&
                    frontItem.GetFrontLayer() == beforeItem?.GetFrontLayer())
                {
                    count++;
                }
            }
            return count == 3;
        }

        public void InitializeSlot()
        {
            itemSlots = new List<ItemSlot>();
            for (int i = 0; i < itemSlotCount; i++)
            {
                var slot = Instantiate(itemSlotPrefab, itemSlotParent);
                itemSlots.Add(slot);
            }
        }

        public BaseItem PushItem()
        {
            int randomIndex = Random.Range(0, itemSlots.Count);
            return itemSlots[randomIndex].PushItem();
        }
    }
}