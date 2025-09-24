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

        public BaseItem PushItem()
        {
            var item = Instantiate(itemPrefab, itemParent);
            items.Push(item);
            item.SetLayer(0);
            foreach (var it in items)
            {
                it.AddLayer(1);
            }
            return item;
        }

        public BaseItem PopItem()
        {
            if (items.Count == 0) return null;
            var item = items.Pop();
            item.SetLayer(5);
            foreach (var it in items)
            {
                it.AddLayer(-1);
            }
            return item;
        }
    }
}