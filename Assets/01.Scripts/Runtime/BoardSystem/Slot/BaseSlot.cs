using System.Collections.Generic;
using MIE.BoardSystem.Item;
using UnityEngine;

namespace MIE.BoardSystem.Slot
{
    public class BaseSlot : MonoBehaviour
    {
        [SerializeField] private BaseItem itemPrefab;
        [SerializeField] private RectTransform itemParent;
        private Stack<BaseItem> items;

        private void Awake()
        {
            items = new Stack<BaseItem>();
            for (int i = 5; i > 0; i--)
            {
                var item = Instantiate(itemPrefab, itemParent);
                items.Push(item);
                item.SetLayer(i);
            }
        }

        public void PushItem(BaseItem item)
        {
            items.Push(item);
            item.SetLayer(0);
            foreach (var it in items)
            {
                it.AddLayer(1);
            }
        }
    }
}