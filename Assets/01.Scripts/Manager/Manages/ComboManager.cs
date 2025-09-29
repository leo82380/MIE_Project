using System;
using MIE.Manager.Interface;
using MIE.Runtime.EventSystem.Core;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;

namespace MIE.Manager.Manages
{
    public class ComboManager : IManager
    {
        private int curCombo;

        public void AddCombo()
        {
            curCombo++;
            EventHandler.TriggerEvent(new ComboEvent(curCombo));
        }
    }

    public struct ComboEvent : IEvent
    {
        public int Combo;

        public ComboEvent(int combo)
        {
            Combo = combo;
        }
    }
}