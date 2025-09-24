namespace MIE.BoardSystem.Slot
{
    public class CountLockSlot : LockSlot
    {
        private int lockCount = 3;

        public override void Unlock()
        {
            lockCount--;
            if (lockCount <= 0)
            {
                lockImage.sprite = null;
                gameObject.SetActive(false);
            }
        }
    }
}