using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;

namespace MIE.Runtime.TimerSystem
{
    public static class Timer
    {
        private static readonly List<TimerHandle> timers = new List<TimerHandle>();

        public static TimerHandle AddTimer(int minutes, int seconds = 0)
        {
            var timer = new TimerHandle(minutes, seconds);
            timers.Add(timer);
            return timer;
        }

        public static void Clear()
        {
            timers.Clear();
        }
    }

    public class TimerHandle
    {
        private float totalSeconds;
        private int minutes;
        private int seconds;

        private TextMeshProUGUI textComponent;

        private event Action OnCompleted;

        public TimerHandle(int minutes = 0, int seconds = 0)
        {
            this.minutes = minutes;
            this.seconds = seconds;
            totalSeconds = minutes * 60 + seconds;
        }

        public TimerHandle Connect(TextMeshProUGUI text)
        {
            textComponent = text;
            UpdateText();
            return this;
        }

        public async void StartTimer(int minutes, int seconds)
        {
            totalSeconds = minutes * 60 + seconds;
            this.minutes = minutes;
            this.seconds = seconds;
            await StartTimerAsync();
        }

        public void UpdateText()
        {
            textComponent.text = $"{minutes:D2}:{seconds:D2}";
        }

        private async Task StartTimerAsync()
        {
            while (totalSeconds > 0)
            {
                await Task.Delay(1000);
                totalSeconds--;
                minutes = (int)(totalSeconds / 60);
                seconds = (int)(totalSeconds % 60);
                UpdateText();
            }

            OnCompleted?.Invoke();
        }

        public TimerHandle OnComplete(Action action)
        {
            OnCompleted += action;
            return this;
        }

        public TimerHandle Start()
        {
            StartTimer(minutes, seconds);
            return this;
        }
    }
}
