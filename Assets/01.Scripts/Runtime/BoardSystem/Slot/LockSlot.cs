using MIE.Runtime.EventSystem.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MIE.Runtime.BoardSystem.Slot
{
    public abstract class LockSlot : MonoBehaviour
    {
        [SerializeField] protected Image lockImage;
        [SerializeField] protected Sprite lockSprite;

        protected bool isLocked = false;

        private void Awake()
        {
            EventHandler.Subscribe<ItemMergedEvent>(HandleMerged);
            lockImage.sprite = lockSprite;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EventHandler.Unsubscribe<ItemMergedEvent>(HandleMerged);
        }

        private void HandleMerged(ItemMergedEvent evt)
        {
            Unlock();
        }

        public void Lock()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            isLocked = true;
        }
        public abstract void Unlock(); 
    }
}