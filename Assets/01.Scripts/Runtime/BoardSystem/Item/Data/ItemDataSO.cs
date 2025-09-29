using MIE.Attribute.PreviewTexture;
using UnityEngine;

namespace MIE.BoardSystem.Item.Data
{
    public class ItemDataSO : ScriptableObject
    {
        [PreviewTexture] public Sprite ItemSprite;
        public int ItemID;
        public EItemType ItemType;
        
    }

    public enum EItemType
    {
        None,
        Milk,
        Coffee,
        Yogurt
    }
}