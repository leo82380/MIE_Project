using MIE.Runtime.EventSystem.Core;
using MIE.Runtime.TimerSystem;
using TMPro;
using UnityEngine;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;

namespace MIE.Runtime.TimerText
{
    public class TimerText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private TimerHandle timer;

        private void Start()
        {
            timer = Timer.AddTimer(1, 10)
                .Connect(timerText)
                .OnComplete(HandleTimerEnd)
                .AddPause()
                .Start();
        }

        private void HandleTimerEnd()
        {
            EventHandler.TriggerEvent<TimerCompleteEvent>();
            timer.RemovePause();
        }
    }

    public struct TimerCompleteEvent : IEvent
    {
        private static TimerCompleteEvent instance = new TimerCompleteEvent();
        public static TimerCompleteEvent Create()
        {
            return instance;
        }
    }
}