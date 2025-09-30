using MIE.Runtime.TimerSystem;
using TMPro;
using UnityEngine;

namespace MIE.Runtime.TimerText
{
    public class TimerText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private void Start()
        {
            Timer.AddTimer(1, 10)
                .Connect(timerText)
                .OnComplete(() => Debug.Log("타이머 완료!"))
                .Start();
        }
    }
}