using System;
using MIE.Manager.Interface;

namespace MIE.Manager.Manages
{
    public class ComboManager : IManager
    {
        private int curCombo;

        public event Action<int> OnComboChanged;
        
        public void AddCombo()
        {
            curCombo++;
            OnComboChanged?.Invoke(curCombo);
        }
    }
}