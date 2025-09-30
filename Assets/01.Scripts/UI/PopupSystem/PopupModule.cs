using System;
using System.Collections.Generic;

namespace MIE.UI.PopupSystem
{
    public class PopupModule : IDisposable
    {
        private Dictionary<PopupType, PopupPanel> popupPanels = new Dictionary<PopupType, PopupPanel>();

        public void RegisterPopup(PopupType type, PopupPanel panel)
        {
            if (!popupPanels.ContainsKey(type))
            {
                popupPanels.Add(type, panel);
            }
        }

        public void OpenPopup(PopupType type)
        {
            if (popupPanels.TryGetValue(type, out var panel))
            {
                panel.OpenPanel();
            }
        }

        public void Dispose()
        {
            popupPanels.Clear();
        }
    }
}