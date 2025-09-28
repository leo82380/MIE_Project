using System;
using System.Collections;
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
                
                slot.OnItemChanged += OnItemSlotChanged;
            }
        }
        
        /// <summary>
        /// ItemSlot의 아이템이 변경되었을 때 호출되는 메서드
        /// </summary>
        private void OnItemSlotChanged(Stack<BaseItem> items)
        {
            StartCoroutine(CheckAndRefreshAllLayers());
        }

        /// <summary>
        /// 랜덤한 아이템 슬롯에 아이템을 생성하고 추가하는 메서드
        /// </summary>
        /// <returns></returns>
        public BaseItem PushItem()
        {
            var availableSlots = itemSlots.Where(slot => slot.GetFrontLayer() != 0).ToList();
            
            if (availableSlots.Count > 0)
            {
                int randomIndex = Random.Range(0, availableSlots.Count);
                return availableSlots[randomIndex].PushItem();
            }
            else
            {
                int randomIndex = Random.Range(0, itemSlots.Count);
                return itemSlots[randomIndex].PushItem();
            }
        }

        public void PushItem(BaseItem item, out Transform parent)
        {
            var availableSlots = itemSlots.Where(slot => slot.GetFrontLayer() != 0).ToList();
            
            if (availableSlots.Count > 0)
            {
                int randomIndex = Random.Range(0, availableSlots.Count);
                var selectedSlot = availableSlots[randomIndex];
                selectedSlot.PushItem(item);
                parent = selectedSlot.transform;
            }
            else
            {
                int randomIndex = Random.Range(0, itemSlots.Count);
                itemSlots[randomIndex].PushItem(item);
                parent = itemSlots[randomIndex].transform;
            }
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
                    item.SpawnMergeEffect();
                    if (item != null)
                    {
                        Destroy(item.gameObject);
                    }
                }
            }
            
            StartCoroutine(CheckAndRefreshAllLayers());
        }
        
        /// <summary>
        /// 0번 레이어가 모두 없으면 전체 ItemSlot의 레이어를 재정렬
        /// </summary>
        public IEnumerator CheckAndRefreshAllLayers()
        {
            yield return null; // 한 프레임 대기
            bool hasLayerZero = false;
            
            // 0번 레이어가 하나라도 있는지 확인
            foreach (var itemSlot in itemSlots)
            {
                if (itemSlot.GetFrontLayer() == 0)
                {
                    hasLayerZero = true;
                    break;
                }
            }
            
            if (!hasLayerZero)
            {
                foreach (var itemSlot in itemSlots)
                {
                    itemSlot.ReduceAllLayers();
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