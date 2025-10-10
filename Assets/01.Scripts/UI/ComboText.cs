using MIE.Manager.Core;
using MIE.Manager.Manages;
using MIE.Runtime.EventSystem.Core;
using TMPro;
using UnityEngine;

namespace MIE.UI
{
    public class ComboText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI comboText;

        private void Start()
        {
            EventHandler.Subscribe<ComboEvent>(HandleComboChanged);
            comboText.text = "Combo 0";
        }

        private void OnDestroy()
        {
            EventHandler.Unsubscribe<ComboEvent>(HandleComboChanged);
        }

        private void HandleComboChanged(ComboEvent evt)
        {
            comboText.text = $"Combo {evt.Combo}, {Managers.Instance.GetManager<ResultManager>().StarCount} Star";
        }
    }
}