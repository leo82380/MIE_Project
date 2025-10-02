using System;
using System.Collections;
using MIE.Runtime.EventSystem.Core;
using UnityEngine;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;

namespace MIE.Runtime.BoosterSystem
{
    public class TimerStopBooster : Booster
    {
        [SerializeField] private float stopDuration = 5f;

        public override void UseBooster()
        {
            StartCoroutine(TimerStopCoroutine());
        }

        private IEnumerator TimerStopCoroutine()
        {
            EventHandler.TriggerEvent(TimerStopEvent.Create(true));
            yield return new WaitForSeconds(stopDuration);
            EventHandler.TriggerEvent(TimerStopEvent.Create(false));
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                UseBooster();
            }
        }
    }

    public struct TimerStopEvent : IEvent
    {
        private bool isStop;

        public bool IsStop => isStop;

        private static TimerStopEvent instance;

        public static TimerStopEvent Create(bool isStop)
        {
            instance.isStop = isStop;
            return instance;
        }

        public TimerStopEvent(bool isStop)
        {
            this.isStop = isStop;
        }
    }
}