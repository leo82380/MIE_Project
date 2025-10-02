using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MIE.Runtime.BoosterSystem;
using TMPro;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;

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
            foreach (var timer in timers)
            {
                timer.StopTimer();
            }
            timers.Clear();
        }

        public static void PauseAll()
        {
            foreach (var timer in timers)
            {
                timer.PauseTimer();
            }
        }

        public static void ResumeAll()
        {
            foreach (var timer in timers)
            {
                timer.ResumeTimer();
            }
        }

        public static void StopAll()
        {
            foreach (var timer in timers)
            {
                timer.StopTimer();
            }
        }
    }

    public class TimerHandle
    {
        private float totalSeconds;
        private int minutes;
        private int seconds;

        private bool isRunning;
        private bool isPaused;

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
            isRunning = true;
            isPaused = false;
            await StartTimerAsync();
        }

        public void StopTimer()
        {
            isRunning = false;
            isPaused = false;
        }

        public void PauseTimer()
        {
            if (isRunning && !isPaused)
            {
                isPaused = true;
            }
        }

        public async void ResumeTimer()
        {
            if (isRunning && isPaused && totalSeconds > 0)
            {
                isPaused = false;
                await StartTimerAsync();
            }
            else if (!isRunning && totalSeconds > 0)
            {
                isRunning = true;
                isPaused = false;
                await StartTimerAsync();
            }
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                ResumeTimer();
            }
            else
            {
                PauseTimer();
            }
        }

        public void UpdateText()
        {
            textComponent.text = $"{minutes:D2}:{seconds:D2}";
        }

        private async Task StartTimerAsync()
        {
            while (totalSeconds > 0 && isRunning)
            {
                if (isPaused)
                {
                    await Task.Delay(100);
                    continue;
                }

                await Task.Delay(1000);

                if (!isRunning || isPaused) break;

                totalSeconds--;
                minutes = (int)(totalSeconds / 60);
                seconds = (int)(totalSeconds % 60);
                UpdateText();
            }

            if (totalSeconds <= 0 && isRunning)
            {
                OnCompleted?.Invoke();
            }
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

        public TimerHandle AddPause()
        {
            EventHandler.Subscribe<TimerStopEvent>(HandlePause);
            return this;
        }

        public TimerHandle RemovePause()
        {
            EventHandler.Unsubscribe<TimerStopEvent>(HandlePause);
            return this;
        }

        private void HandlePause(TimerStopEvent evt)
        {
            if (evt.IsStop)
            {
                PauseTimer();
            }
            else
            {
                ResumeTimer();
            }
        }

        ~TimerHandle()
        {
            StopTimer();
            EventHandler.Unsubscribe<TimerStopEvent>(HandlePause);
        }
    }
}
