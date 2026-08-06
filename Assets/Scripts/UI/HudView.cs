using System.Collections;
using TMPro;
using UnityEngine;
using GemRush.Gameplay;

namespace GemRush.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text matchEndedText;

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
            scoreText.text = "0";
        }

        private void OnEnable()
        {
            ScoreCounter.OnScoreChanged += HandleScoreChanged;
            MatchTimer.OnTimeChanged += HandleTimeChanged;
            MatchTimer.OnMatchEnded += HandleMatchEnded;
        }

        private void OnDisable()
        {
            ScoreCounter.OnScoreChanged -= HandleScoreChanged;
            MatchTimer.OnTimeChanged -= HandleTimeChanged;
            MatchTimer.OnMatchEnded -= HandleMatchEnded;
        }

        private void HandleScoreChanged(int score)
        {
            scoreText.text = score.ToString();
            if (_punch != null) StopCoroutine(_punch);
            _punch = StartCoroutine(PunchScale(scoreText.transform));
        }

        private void HandleTimeChanged(float timeRemaining)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
            timerText.color = timeRemaining <= lowTimeThreshold ? lowTimeColor : _defaultTimerColor;
        }

        private void HandleMatchEnded() => matchEndedText.gameObject.SetActive(true);

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