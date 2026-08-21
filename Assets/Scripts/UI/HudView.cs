using System.Collections;
using TMPro;
using UnityEngine;
using GemRush.Gameplay;
using GemRush.Core.Events;
using GemRush.Core;

namespace GemRush.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text matchEndedText;

        [Header("Listens To")]
        [SerializeField] private IntEventChannelSO scoreChangedChannel;
        [SerializeField] private FloatEventChannelSO timeChangedChannel;
        [SerializeField] private MatchStateEventChannelSO stateChannel;

        [Header("Feedback")]
        [SerializeField] private float lowTimeThreshold = 10f;
        [SerializeField] private Color lowTimeColor = new(1f, 0.35f, 0.3f);
        [SerializeField] private float punchScale = 1.3f;
        [SerializeField] private float punchDuration = 0.15f;

        private Color _defaultTimerColor;
        private Coroutine _punch;

        private void Awake()
        {
            _defaultTimerColor = timerText.color;
            matchEndedText.gameObject.SetActive(false);
            scoreText.text = "Score: 0";
        }

        private void OnEnable()
        {
            scoreChangedChannel.Subscribe(HandleScoreChanged);
            timeChangedChannel.Subscribe(HandleTimeChanged);
            stateChannel.Subscribe(HandleStateChanged);
        }

        private void OnDisable()
        {
            scoreChangedChannel.Unsubscribe(HandleScoreChanged);
            timeChangedChannel.Unsubscribe(HandleTimeChanged);
            stateChannel.Unsubscribe(HandleStateChanged);
        }

        private void HandleStateChanged(MatchState state)
        {
            matchEndedText.gameObject.SetActive(state == MatchState.Ended);
        }

        private void HandleScoreChanged(int score)
        {
            scoreText.text = "Score: " + score.ToString();
            if (_punch != null) StopCoroutine(_punch);
            _punch = StartCoroutine(PunchScale(scoreText.transform));
        }

        private void HandleTimeChanged(float timeRemaining)
        {
            timerText.text = "Timer: " + Mathf.CeilToInt(timeRemaining).ToString();
            timerText.color = timeRemaining <= lowTimeThreshold ? lowTimeColor : _defaultTimerColor;
        }

        private IEnumerator PunchScale(Transform target)
        {
            float half = punchDuration * 0.5f;
            for (float t = 0f; t < punchDuration; t += Time.deltaTime)
            {
                float k = t < half ? t / half : 1f - (t - half) / half;
                target.localScale = Vector3.one * Mathf.LerpUnclamped(1f, punchScale, k);
                yield return null;
            }
            target.localScale = Vector3.one;
        }
    }
}