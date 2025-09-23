using System;
using UnityEngine;
using UnityEngine.UI;

namespace MIE.BoardSystem.Item.Component
{
    public class ItemVisual : MonoBehaviour, IItemComponent
    {
        [SerializeField] private Image itemVisual;

        public void RegisterEvent(IItem item)
        {
            item.OnEnableItem += HandleEnableItem;
        }

        public void UnregisterEvent(IItem item)
        {
            item.OnEnableItem -= HandleEnableItem;
        }

        private void HandleEnableItem(int layer)
        {
            float brightness = Mathf.Lerp(1.0f, 0.3f, layer / 10.0f);
            brightness = Mathf.Clamp(brightness, 0.3f, 1.0f);
            
            itemVisual.color = new Color(brightness, brightness, brightness, 1.0f);
        }
    }
}