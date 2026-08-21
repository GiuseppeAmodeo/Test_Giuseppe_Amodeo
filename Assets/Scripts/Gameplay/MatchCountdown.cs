using System;
using System.Collections;
using TMPro;
using UnityEngine;
using GemRush.Core;
using GemRush.Core.Events;

namespace GemRush.Gameplay
{
    public class MatchCountdown : MonoBehaviour
    {
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private float stepDuration = 0.8f;

        [Header("Listens To")]
        [SerializeField] private MatchStateEventChannelSO stateChannel;

        [Header("Broadcasts To")]
        [SerializeField] private VoidEventChannelSO countdownFinishChannel;

        private void OnEnable()
        {
            stateChannel.Subscribe(HandleStateChanged);
        }


        private void OnDisable()
        {
            stateChannel.Unsubscribe(HandleStateChanged);
        }

        private void HandleStateChanged(MatchState state)
        {
            if (state == MatchState.Countdown)
                StartCoroutine(RunCountdown());
        }

        private IEnumerator RunCountdown()
        {
            countdownText.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return AnimateStep();
            }

            countdownText.text = "GO!";
            yield return AnimateStep();

            countdownText.gameObject.SetActive(false);

            Debug.Log("[Countdown] alzo countdownFinished");

            countdownFinishChannel.Raise();
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