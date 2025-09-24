using System;
using System.Collections.Generic;
using MIE.BoardSystem.Item.Component;
using MIE.BoardSystem.Item.Data;
using UnityEngine;

namespace MIE.BoardSystem.Item
{
    // 아이템의 기본 클래스
    public class BaseItem : MonoBehaviour, IItem
    {
        private ItemDataSO itemData;
        private Dictionary<Type, IItemComponent> componentDict;

        public event Action<int> OnEnableItem;
        public event Action<ItemDataSO> OnSetItemData;

        public int Layer { get; private set; }

        private void Awake()
        {
            componentDict = new Dictionary<Type, IItemComponent>();
            var components = GetComponents<IItemComponent>();
            foreach (var component in components)
            {
                var type = component.GetType();
                if (!componentDict.ContainsKey(type))
                {
                    componentDict[type] = component;
                    component.RegisterEvent(this);
                }
                else
                {
                    Debug.LogWarning($"Component of type {type} already exists on {gameObject.name}.");
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var component in componentDict.Values)
            {
                component.UnregisterEvent(this);
            }
            componentDict.Clear();
        }

        public void SetLayer(int layer)
        {
            this.Layer = layer;
            OnEnableItem?.Invoke(layer);
        }

        public void AddLayer(int addValue)
        {
            OnEnableItem?.Invoke(Layer + addValue);
        }

        public void SetItemData(ItemDataSO data)
        {
            itemData = data;
            gameObject.name = $"Item_{data.ItemID}_{data.ItemType}";
            OnSetItemData?.Invoke(data);
        }

        public ItemDataSO GetItemData()
        {
            return itemData;
        }
    }
}