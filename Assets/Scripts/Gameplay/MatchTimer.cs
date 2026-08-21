using System;
using UnityEngine;
using GemRush.Core;
using GemRush.Core.Events;

namespace GemRush.Gameplay
{
    public class MatchTimer : MonoBehaviour
    {
        [SerializeField] private GameConfig config;

        [Header("Listens To")]
        [SerializeField] private MatchStateEventChannelSO stateChannel;

        [Header("Broadcasts To")]
        [SerializeField] private FloatEventChannelSO timeChangedChannel;
        [SerializeField] private VoidEventChannelSO timeExpiredChannel;

        public float TimeRemaining { get; private set; }

        public bool IsRunning { get; private set; }

        private void OnEnable() => stateChannel.Subscribe(HandleStateChanged);
        private void OnDisable() => stateChannel.Unsubscribe(HandleStateChanged);

        private void HandleStateChanged(MatchState state)
        {
            if (state == MatchState.Playing)
            {
                TimeRemaining = config.matchDuration;
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }
        }

        private void Update()
        {
            if (!IsRunning) return;

            TimeRemaining = Mathf.Max(0f, TimeRemaining - Time.deltaTime);
            timeChangedChannel.Raise(TimeRemaining);

            if (TimeRemaining <= 0f)
            {
                IsRunning = false;
                timeExpiredChannel.Raise();
            }
        }
    }
}