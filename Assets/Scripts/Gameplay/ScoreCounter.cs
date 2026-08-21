using System;
using UnityEngine;
using GemRush.Core;
using GemRush.Core.Events;

namespace GemRush.Gameplay
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private GameConfig config;

        [SerializeField] private GemEventChannelSO gemEventChannelSO;

        [SerializeField] private IntEventChannelSO scoreChangedChannel;

        public int CurrentScore { get; private set; }

        private void OnEnable() => gemEventChannelSO.Subscribe(HandleGemCollected);
        private void OnDisable() => gemEventChannelSO.Unsubscribe(HandleGemCollected);

        private void HandleGemCollected(Gem gem)
        {
            CurrentScore += config.gemValue;
            scoreChangedChannel.Raise(CurrentScore);

            //OnScoreChanged?.Invoke(CurrentScore);
        }
    }
}