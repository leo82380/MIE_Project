using MIE.UI.Component;
using UnityEngine;

namespace MIE.UI.PopupSystem
{
    public class PopupPanel : FadePanel
    {
        [SerializeField] private PopupType popupType;
        public PopupType PopupType => popupType;
    }
}