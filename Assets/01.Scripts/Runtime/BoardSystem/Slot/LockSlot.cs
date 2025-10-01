using UnityEngine;
using UnityEngine.UI;

namespace MIE.Runtime.BoardSystem.Slot
{
    public abstract class LockSlot : MonoBehaviour
    {
        [SerializeField] protected Image lockImage;
        [SerializeField] protected Sprite lockSprite;
        
        public void Lock()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
        public abstract void Unlock(); 
    }
}