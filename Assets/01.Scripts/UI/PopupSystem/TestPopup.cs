using MIE.Manager.Core;
using MIE.Manager.Manages;
using UnityEngine;

namespace MIE.UI.PopupSystem
{    
    public class TestPopup : MonoBehaviour
    {
        private void Start()
        {
            Managers.Instance.GetManager<UIManager>().GetUIModule<PopupModule>().OpenPopup(PopupType.GameOver);
        }
    }
}