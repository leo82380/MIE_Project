using System;
using MIE.Manager.Core;
using MIE.Manager.Interface;
using UnityEngine;

namespace MIE.Manager.Manages
{
    public class ResultManager : IManager, IInitializable, IDisposable
    {
        private int starCount = 0;

        public int StarCount => starCount;

        public event Action<int> OnStarCountChanged;

        public void AddStar(int combo)
        {
            int increment = GetIncrement(combo);
            starCount += increment;
            OnStarCountChanged?.Invoke(starCount);
            Debug.Log($"[ResultManager] Combo: {combo}, Increment: {increment}, Total Stars: {starCount}");
        }

        private int GetIncrement(int combo)
        {
            int currentCombo = combo;
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
            Managers.Instance.GetManager<ComboManager>().OnComboChanged += AddStar;
        }

        public void Dispose()
        {
            Managers.Instance.GetManager<ComboManager>().OnComboChanged -= AddStar;
        }

        public void ResetStars()
        {
            starCount = 0;
        }
    }
}