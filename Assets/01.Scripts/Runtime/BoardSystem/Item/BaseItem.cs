using System;
using System.Collections.Generic;
using MIE.BoardSystem.Item.Component;
using UnityEngine;

namespace MIE.BoardSystem.Item
{
    public class BaseItem : MonoBehaviour, IItem
    {
        private Dictionary<Type, IItemComponent> componentDict;

        public event Action<int> OnEnableItem;
        private int layer;

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
            this.layer = layer;
            OnEnableItem?.Invoke(layer);
        }

        public void AddLayer(int addValue)
        {
            OnEnableItem?.Invoke(layer + addValue);
        }
    }
}