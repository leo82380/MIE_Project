using System;
using System.Collections.Generic;
using MIE.Manager.Interface;

namespace MIE.UI.FadeSystem
{
    public class FadeModule : IUIModule
    {
        private Dictionary<string, FadePanel> fadePanels;

        public void Initialize()
        {
            fadePanels = new Dictionary<string, FadePanel>();
        }

        public void Dispose()
        {
            fadePanels.Clear();
        }

        public void RegisterFadePanel(string key, FadePanel panel)
        {
            if (!fadePanels.ContainsKey(key))
            {
                fadePanels.Add(key, panel);
            }
        }

        public void OpenPanel(string key, Action onComplete = null)
        {
            if (fadePanels.TryGetValue(key, out var panel))
            {
                panel.OpenPanel(onComplete);
            }
        }

        public void ClosePanel(string key, Action onComplete = null)
        {
            if (fadePanels.TryGetValue(key, out var panel))
            {
                panel.ClosePanel(onComplete);
            }
        }
    }
}