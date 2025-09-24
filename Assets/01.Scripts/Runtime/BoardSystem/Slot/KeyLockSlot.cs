namespace MIE.BoardSystem.Slot
{
    public class KeyLockSlot : LockSlot
    {
        public override void Unlock()
        {
            lockImage.sprite = null;
            gameObject.SetActive(false);
        }
    }
}