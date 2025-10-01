using MIE.Manager.Core;
using MIE.Manager.Manages;
using UnityEngine;

namespace MIE.UI.FadeSystem
{    
    public class FadeRegisterModule : MonoBehaviour
    {
        private void Start()
        {
            var fadePanel = GetComponent<FadePanel>();
            if (fadePanel != null)
            {
                Managers.Instance.GetManager<UIManager>().GetUIModule<FadeModule>().RegisterFadePanel(fadePanel.name, fadePanel);
            }
        }
    }
}