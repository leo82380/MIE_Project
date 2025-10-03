using System;
using System.Collections.Generic;
using MIE.Manager.Interface;
using MIE.Runtime.BoardSystem.Item;
using MIE.Runtime.BoardSystem.Item.Data;

namespace MIE.Manager.Manages
{
    public class BoardManager : IManager, IInitializable, IDisposable
    {
        private List<BaseItem> boardItems;

        public void Initialize()
        {
            boardItems = new List<BaseItem>();
        }

        public void Dispose()
        {
            boardItems.Clear();
        }

        public void RegisterItem(BaseItem item)
        {
            boardItems.Add(item);
        }

        public void UnregisterItem(BaseItem item)
        {
            boardItems.Remove(item);
        }

        public BaseItem GetItem(int layer, EItemType itemType)
        {
            foreach (var item in boardItems)
            {
                if (item.Layer == layer && item.GetItemData().ItemType == itemType)
                {
                    return item;
                }
            }
            return null;
        }
    }
}