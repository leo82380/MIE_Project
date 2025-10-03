using UnityEngine;
using UnityEngine.EventSystems;

namespace MIE.Runtime.BoosterSystem
{
    public class BoosterUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Booster booster;

        public void OnPointerClick(PointerEventData eventData)
        {
            booster.UseBooster();
        }
    }
}