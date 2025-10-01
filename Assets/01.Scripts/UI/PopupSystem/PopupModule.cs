using System;
using System.Collections.Generic;
using MIE.Manager.Interface;
using Object = UnityEngine.Object;

namespace MIE.UI.PopupSystem
{
    public class PopupModule : IUIModule
    {
        private Dictionary<PopupType, PopupPanel> popupPanels;

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

        public void Initialize()
        {
            popupPanels = new Dictionary<PopupType, PopupPanel>();
            foreach (PopupPanel panel in Object.FindObjectsByType<PopupPanel>(UnityEngine.FindObjectsSortMode.None))
            {
                RegisterPopup(panel.PopupType, panel);
            }
        }

        public void Dispose()
        {
            popupPanels.Clear();
        }

        
    }
}