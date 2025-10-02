using System;
using System.Collections.Generic;
using MIE.Manager.Interface;
using MIE.Runtime.BoardSystem.Item;

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
    }
}