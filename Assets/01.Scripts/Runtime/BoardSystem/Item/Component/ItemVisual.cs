using MIE.BoardSystem.Item.Data;
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
            item.OnSetItemData += HandleSetItemData;
        }

        public void UnregisterEvent(IItem item)
        {
            item.OnEnableItem -= HandleEnableItem;
            item.OnSetItemData -= HandleSetItemData;
        }

        private void HandleSetItemData(ItemDataSO so)
        {
            if (!so.ItemSprite)
            {
                Debug.LogWarning($"ItemDataSO {so.name} does not have an ItemSprite assigned.");
                return;
            }
            SetItemSprite(so.ItemSprite);
        }

        private void HandleEnableItem(int layer)
        {
            float brightness = Mathf.Lerp(1.0f, 0f, layer / 5.0f);
            brightness = Mathf.Clamp(brightness, 0f, 1.0f);

            itemVisual.color = new Color(brightness, brightness, brightness, 1.0f);
        }

        public void SetItemSprite(Sprite sprite)
        {
            itemVisual.sprite = sprite;
        }
    }
}