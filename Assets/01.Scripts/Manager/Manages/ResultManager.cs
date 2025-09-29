using System;
using MIE.Manager.Interface;
using MIE.Runtime.EventSystem.Core;
using UnityEngine;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;

namespace MIE.Manager.Manages
{
    public class ResultManager : IManager, IInitializable, IDisposable
    {
        private int starCount = 0;

        public int StarCount => starCount;

        public void AddStar(ComboEvent comboEvent)
        {
            int increment = GetIncrement(comboEvent);
            starCount += increment;
            EventHandler.TriggerEvent(new StarEvent(starCount));
            Debug.Log($"[ResultManager] Combo: {comboEvent.Combo}, Increment: {increment}, Total Stars: {starCount}");
        }

        private int GetIncrement(ComboEvent comboEvent)
        {
            int currentCombo = comboEvent.Combo;
            int increment = 1;
            int groupSize = 2; // 첫 번째 그룹 크기

            while (currentCombo > groupSize)
            {
                currentCombo -= groupSize;
                increment++;
                groupSize++; // 다음 그룹은 1개씩 더 큼
            }

            return increment;
        }

        public void Initialize()
        {
            EventHandler.Subscribe<ComboEvent>(AddStar);
        }

        public void Dispose()
        {
            EventHandler.Unsubscribe<ComboEvent>(AddStar);
        }

        public void ResetStars()
        {
            starCount = 0;
        }
    }

    public struct StarEvent : IEvent
    {
        public int StarCount;

        public StarEvent(int starCount)
        {
            StarCount = starCount;
        }
    }
}