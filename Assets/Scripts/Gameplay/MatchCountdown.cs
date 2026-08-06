using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GemRush.Gameplay
{
    public class MatchCountdown : MonoBehaviour
    {
        public static event Action OnCountdownFinished;

        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private float stepDuration = 0.8f;

        private void Start() => StartCoroutine(RunCountdown());

        private IEnumerator RunCountdown()
        {
            Time.timeScale = 0f; // freeze gameplay, coroutine uses realtime
            countdownText.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return AnimateStep();
            }

            countdownText.text = "GO!";
            yield return AnimateStep();

            countdownText.gameObject.SetActive(false);
            Time.timeScale = 1f;
            OnCountdownFinished?.Invoke();
        }

        private IEnumerator AnimateStep()
        {
            Transform t = countdownText.transform;
            for (float e = 0f; e < stepDuration; e += Time.unscaledDeltaTime)
            {
                float k = 1f - Mathf.Pow(1f - Mathf.Clamp01(e / stepDuration), 3f); // ease-out cubic
                t.localScale = Vector3.one * Mathf.Lerp(1.6f, 1f, k);
                countdownText.alpha = Mathf.Lerp(1f, 0.2f, k);
                yield return null;
            }
        }
    }
}