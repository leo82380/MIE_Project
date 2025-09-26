using System;
using System.Collections.Generic;
using System.Linq;
using MIE.BoardSystem.Item;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MIE.BoardSystem.Slot
{
    // 기본 슬롯 클래스
    public class BaseSlot : MonoBehaviour
    {
        [SerializeField] private int itemSlotCount = 3;
        [SerializeField] private ItemSlot itemSlotPrefab;
        [SerializeField] private RectTransform itemSlotParent;
        [SerializeField] private LockSlot lockImage;
        [SerializeField] private bool isLocked;
        private List<ItemSlot> itemSlots;


        private void Awake()
        {
            InitializeSlot();
        }

        private void Start()
        {
            itemSlots.ForEach(slot => slot.OnItemChanged += HandleItemChanged);
        }

        private void OnDestroy()
        {
            itemSlots.ForEach(slot => slot.OnItemChanged -= HandleItemChanged);
        }

        private void HandleItemChanged(Stack<BaseItem> stack)
        {
            if (CheckMerge())
            {
                ExecuteMerge();
            }
        }

        public bool CheckMerge()
        {
            var layerZeroItems = new List<int>();

            foreach (var itemSlot in itemSlots)
            {
                var frontItem = itemSlot.GetFront();
                var frontLayer = itemSlot.GetFrontLayer();

                // 0번 레이어이고 아이템이 존재하는 경우만 추가
                if (frontLayer == 0 && frontItem != null)
                {
                    layerZeroItems.Add(frontItem.ItemID);
                }
            }

            if (layerZeroItems.Count == 3)
            {
                var firstItemID = layerZeroItems[0];
                return layerZeroItems.All(id => id == firstItemID);
            }

            return false;
        }

        /// <summary>
        /// 슬롯 초기화 메서드
        /// </summary>
        public void InitializeSlot()
        {
            lockImage.gameObject.SetActive(isLocked);
            itemSlots = new List<ItemSlot>();
            for (int i = 0; i < itemSlotCount; i++)
            {
                var slot = Instantiate(itemSlotPrefab, itemSlotParent);
                itemSlots.Add(slot);
            }
        }

        /// <summary>
        /// 랜덤한 아이템 슬롯에 아이템을 생성하고 추가하는 메서드
        /// </summary>
        /// <returns></returns>
        public BaseItem PushItem()
        {
            int randomIndex = Random.Range(0, itemSlots.Count);
            return itemSlots[randomIndex].PushItem();
        }

        public void PushItem(BaseItem item, out Transform parent)
        {
            int randomIndex = Random.Range(0, itemSlots.Count);
            itemSlots[randomIndex].PushItem(item);
            parent = itemSlots[randomIndex].transform;
        }

        /// <summary>
        /// 머지된 아이템들을 제거하는 메서드
        /// </summary>
        public void ExecuteMerge()
        {
            if (!CheckMerge()) return;
            
            // 0번 레이어 아이템들을 모두 제거
            foreach (var itemSlot in itemSlots)
            {
                if (itemSlot.GetFrontLayer() == 0)
                {
                    var item = itemSlot.PopItem();
                    if (item != null)
                    {
                        Destroy(item.gameObject);
                    }
                }
            }
        }

        public bool IsFull()
        {
            // 모든 아이템 슬롯이 가득 차있는지 확인
            // (각 슬롯에 최소 하나 이상의 아이템이 있어야 함)
            foreach (var slot in itemSlots)
            {
                if (slot.IsEmpty()) return false; // 빈 슬롯이 하나라도 있으면 full이 아님
            }
            return true;
        }

    }

    public enum LockType
    {
        None, // Unlocked
        Key,  // Locked with a key
        Count // Locked with a count
    }
}