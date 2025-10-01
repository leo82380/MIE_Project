using System.Collections.Generic;
using MIE.Runtime.BoardSystem.Item.Data;
using MIE.Runtime.BoardSystem.Slot;
using UnityEngine;

namespace MIE.Runtime.BoardSystem
{
    // 보드 매니저 클래스
    public class Board : MonoBehaviour
    {
        [SerializeField] private List<BaseSlot> slots;
        [SerializeField] private List<ItemDataSO> itemDataSOs;
        [SerializeField] private int matchCount;

        public void Start()
        {
            InitializeBoard();
            
        }
        
        /// <summary>
        /// 모든 슬롯에서 머지 가능한 것들을 확인하고 실행
        /// </summary>
        public void CheckAndExecuteAllMerges()
        {
            foreach (var slot in slots)
            {
                if (slot.CheckMerge())
                {
                    slot.ExecuteMerge();
                }
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
                    item.SetItemData(itemData);
                }
            }
        }
    }
}
