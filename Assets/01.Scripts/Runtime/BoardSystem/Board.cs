using System.Collections.Generic;
using MIE.BoardSystem.Item.Data;
using MIE.BoardSystem.Slot;
using UnityEngine;

namespace MIE.BoardSystem
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private List<BaseSlot> slots;
        [SerializeField] private List<ItemDataSO> itemDataSOs;
        [SerializeField] private int matchCount;

        public void Start()
        {
            InitializeBoard();
        }

        // 아이템 생성 및 슬롯에 배치
        private void InitializeBoard()
        {
            for (int i = 0; i < matchCount; i++)
            {
                var itemIndex = Random.Range(0, itemDataSOs.Count);
                var itemData = itemDataSOs[itemIndex];

                // 무조건 3개씩 생성
                for (int j = 0; j < 3; j++)
                {
                    var slotIndex = Random.Range(0, slots.Count);
                    var slot = slots[slotIndex];
                    var item = slot.PushItem();
                    item.SetItemData(itemData);
                }
            }
        }
    }
}
