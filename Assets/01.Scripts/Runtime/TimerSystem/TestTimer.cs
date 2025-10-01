using System;
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
                .OnComplete(HandleTimerEnd)
                .Start();
        }

        private void HandleTimerEnd()
        {
            EventSystem.Core.EventHandler.TriggerEvent(new TimerCompleteEvent());
        }
    }

    public struct TimerCompleteEvent : EventSystem.Core.IEvent {}
}