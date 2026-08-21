using UnityEngine;
using GemRush.Core.Events;
using UnityEngine.XR;

namespace GemRush.Core
{

    public class MatchStateController : MonoBehaviour
    {
        [Header("Listens To")]
        [SerializeField] private VoidEventChannelSO countdownFinishChannel;
        [SerializeField] private VoidEventChannelSO timerExpiredChannel;


        [Header("Broadcasts To")]
        [SerializeField] private MatchStateEventChannelSO stateChannel;


        public MatchState State { get; private set; }

        private void OnEnable()
        {
            countdownFinishChannel.Subscribe(HandleCountdownFinished);
            timerExpiredChannel.Subscribe(HandleTimerExpired);
        }

        private void OnDisable()
        {
            countdownFinishChannel.Unsubscribe(HandleCountdownFinished);
            timerExpiredChannel.Unsubscribe(HandleTimerExpired);
        }

        private void Start()
        {
            ChangeState(MatchState.Countdown);
        }

        private void HandleCountdownFinished()
        {
            if (State == MatchState.Countdown)
                ChangeState(MatchState.Playing);
        }

        private void HandleTimerExpired()
        {
            if (State == MatchState.Playing)
                ChangeState(MatchState.Ended);
        }

        private void ChangeState(MatchState next)
        {
            if (State == next) return;

            Debug.Log($"[FSM] {State} -> {next}");


            State = next;
            stateChannel.Raise(next);
        }
    }

}