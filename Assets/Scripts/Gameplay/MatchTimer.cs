using System;
using UnityEngine;
using GemRush.Core;

namespace GemRush.Gameplay
{
    public class MatchTimer : MonoBehaviour
    {
        public static event Action<float> OnTimeChanged;
        public static event Action OnMatchEnded;

        [SerializeField] private GameConfig config;

        public float TimeRemaining { get; private set; }
        public bool IsRunning { get; private set; }

        private void OnEnable() => MatchCountdown.OnCountdownFinished += StartMatch;
        private void OnDisable() => MatchCountdown.OnCountdownFinished -= StartMatch;

        private void StartMatch()
        {
            TimeRemaining = config.matchDuration;
            IsRunning = true;
        }

        private void Update()
        {
            if (!IsRunning) return;

            TimeRemaining = Mathf.Max(0f, TimeRemaining - Time.deltaTime);
            OnTimeChanged?.Invoke(TimeRemaining);

            if (TimeRemaining <= 0f)
            {
                IsRunning = false;
                OnMatchEnded?.Invoke();
            }
        }
    }
}