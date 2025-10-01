using System;
using MIE.Runtime.BoardSystem.Item.Data;

namespace MIE.Runtime.BoardSystem.Item
{
    public interface IItem
    {
        event Action<int> OnEnableItem;
        event Action<ItemDataSO> OnSetItemData;
    }
}