using System;
using UnityEngine;
using GemRush.Core;

namespace GemRush.Gameplay
{
    public class ScoreCounter : MonoBehaviour
    {
        public static event Action<int> OnScoreChanged;

        [SerializeField] private GameConfig config;

        public int CurrentScore { get; private set; }

        private void OnEnable() => Gem.OnCollected += HandleGemCollected;
        private void OnDisable() => Gem.OnCollected -= HandleGemCollected;

        private void HandleGemCollected(Gem gem)
        {
            CurrentScore += config.gemValue;
            OnScoreChanged?.Invoke(CurrentScore);
        }
    }
}