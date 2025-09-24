using System;
using MIE.BoardSystem.Item.Data;

namespace MIE.BoardSystem.Item
{
    public interface IItem
    {
        event Action<int> OnEnableItem;
        event Action<ItemDataSO> OnSetItemData;
    }
}