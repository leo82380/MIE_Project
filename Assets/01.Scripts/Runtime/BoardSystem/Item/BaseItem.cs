using System;
using System.Collections.Generic;
using MIE.Manager.Interface;
using MIE.Runtime.BoardSystem.Item.Component;
using MIE.Runtime.BoardSystem.Item.Data;
using UnityEngine;

namespace MIE.Runtime.BoardSystem.Item
{
    // 아이템의 기본 클래스
    public class BaseItem : MonoBehaviour, IItem
    {
        [SerializeField] private ParticleSystem mergeEffect;

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
                    if (component is IInitializable initializable)
                    {
                        initializable.Initialize();
                    }
                }
                else
                {
                    Debug.LogWarning($"Component of type {type} already exists on {gameObject.name}.");
                }
            }
        }

        public void SpawnMergeEffect()
        {
            Instantiate(mergeEffect, transform.position, Quaternion.identity);
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
            Layer = layer;
            transform.localScale = Vector3.one * (1 - Layer * 0.1f);
            OnEnableItem?.Invoke(Layer);
        }

        public void AddLayer(int addValue)
        {
            Layer += addValue;
            transform.localScale = Vector3.one * (1 - Layer * 0.1f);
            OnEnableItem?.Invoke(Layer);
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