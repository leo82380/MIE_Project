using UnityEngine;
using UnityEngine.UI;

namespace MIE.BoardSystem.Slot
{
    public abstract class LockSlot : MonoBehaviour
    {
        [SerializeField] protected Image lockImage;
        [SerializeField] protected Sprite lockSprite;
        
        public void SetUp()
        {
            transform.SetAsLastSibling();
        }
        public abstract void Unlock(); 
    }
}