using System;
using MIE.Manager.Interface;
using MIE.Runtime.EventSystem.Core;
using MIE.UI.PopupSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MIE.Manager.Manages
{
    public class UIManager : IManager, IInitializable, IDisposable
    {
        private PopupModule popupModule;

        public PopupModule PopupModule => popupModule;

        public void Initialize()
        {
            popupModule = new PopupModule();
            foreach (PopupPanel panel in Object.FindObjectsByType<PopupPanel>(FindObjectsSortMode.None))
            {
                popupModule.RegisterPopup(panel.PopupType, panel);
            }

            Runtime.EventSystem.Core.EventHandler.Subscribe<GameStateEvent>(HandleStateChange);
        }

        public void Dispose()
        {
            popupModule.Dispose();
            Runtime.EventSystem.Core.EventHandler.Unsubscribe<GameStateEvent>(HandleStateChange);
        }

        private void HandleStateChange(GameStateEvent t)
        {
            if (t.State == GameState.GameOver)
            {
                popupModule.OpenPopup(PopupType.GameOver);
            }
        }
    }
}