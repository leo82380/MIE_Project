namespace MIE.BoardSystem.Item.Component
{
    public interface IItemComponent
    {
        void RegisterEvent(IItem item);
        void UnregisterEvent(IItem item);
    }
}