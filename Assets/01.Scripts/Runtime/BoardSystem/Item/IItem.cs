using System;

namespace MIE.BoardSystem.Item
{
    public interface IItem
    {
        event Action<int> OnEnableItem;
    }
}