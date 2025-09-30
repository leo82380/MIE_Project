using System;
using MIE.Manager.Interface;
using MIE.UI.PopupSystem;

namespace MIE.Manager.Manages
{
    public class UIManager : IManager, IInitializable, IDisposable
    {
        private PopupModule popupModule;

        public PopupModule PopupModule => popupModule;

        public void Initialize()
        {
            popupModule = new PopupModule();
            foreach (PopupPanel panel in UnityEngine.Object.FindObjectsByType<PopupPanel>(UnityEngine.FindObjectsSortMode.None))
            {
                popupModule.RegisterPopup(panel.PopupType, panel);
            }
        }

        public void Dispose()
        {
            popupModule.Dispose();
        }
    }
}