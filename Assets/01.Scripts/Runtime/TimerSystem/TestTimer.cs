using MIE.Runtime.TimerSystem;
using TMPro;
using UnityEngine;

namespace MIE.Runtime.TimerText
{
    public class TestTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI2;

        private void Start()
        {
            Timer.AddTimer(1, 10)
                .Connect(textMeshProUGUI)
                .OnComplete(() => Debug.Log("타이머 완료!"))
                .Start();
        }
    }
}