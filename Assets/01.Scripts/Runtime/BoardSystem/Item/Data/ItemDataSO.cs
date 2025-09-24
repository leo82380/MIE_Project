using UnityEngine;

namespace MIE.BoardSystem.Item.Data
{
    public class ItemDataSO : ScriptableObject
    {
        public int ItemID;
        public EItemType ItemType;
        public Sprite ItemSprite;
    }

    public enum EItemType
    {
        None,
        Milk,
        Coffee,
        Yogurt
    }
}