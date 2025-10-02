using System.Collections.Generic;
using MIE.Manager.Core;
using MIE.Manager.Manages;
using MIE.Runtime.BoardSystem.Item;
using MIE.Runtime.BoardSystem.Item.Data;
using MIE.Runtime.BoardSystem.Slot;
using MIE.Runtime.EventSystem.Core;
using UnityEngine;

namespace MIE.Runtime.BoardSystem
{
    // 보드 매니저 클래스
    public class Board : MonoBehaviour
    {
        [SerializeField] private List<BaseSlot> slots;
        [SerializeField] private List<ItemDataSO> itemDataSOs;
        [SerializeField] private int matchCount;

        private int remainItemCount;

        public void Start()
        {
            InitializeBoard();
            EventHandler.Subscribe<ItemMergedEvent>(HandleItemMerged);
            remainItemCount = matchCount * 3;
        }

        private void OnDestroy()
        {
            EventHandler.Unsubscribe<ItemMergedEvent>(HandleItemMerged);
        }

        private void HandleItemMerged(ItemMergedEvent evt)
        {
            remainItemCount -= evt.mergeCount;
            if (remainItemCount <= 0)
            {
                EventHandler.TriggerEvent<BoardClearedEvent>();
            }
        }

        /// <summary>
        /// 보드 초기화 메서드
        /// </summary>
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
                    Managers.Instance.GetManager<BoardManager>().RegisterItem(item);
                    item.SetItemData(itemData);
                }
            }
        }
    }

    public struct BoardClearedEvent : IEvent
    {
        private static BoardClearedEvent instance;

        public static BoardClearedEvent Create()
        {
            return instance;
        }
    }
}
