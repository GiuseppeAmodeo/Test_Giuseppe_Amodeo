using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GemRush.Core;
using GemRush.Core.Events;

namespace GemRush.Gameplay
{
    /// <summary>
    /// Owns the end-of-match sequence: persists the high score,
    /// stores session data and transitions to the Results scene.
    /// </summary>
    public class MatchFlowController : MonoBehaviour
    {
        [SerializeField] private ScoreCounter scoreCounter;
        [SerializeField] private float resultsDelay = 1.5f;

        [Header("Listens To")]
        [SerializeField] private MatchStateEventChannelSO stateChannel;

        private readonly IHighScoreRepository _highScores = new PlayerPrefsHighScoreRepository();

        private void OnEnable() => stateChannel.Subscribe(HandleStateChanged);
        private void OnDisable() => stateChannel.Unsubscribe(HandleStateChanged);

        private void HandleStateChanged(MatchState state)
        {
            if (state != MatchState.Ended) return;

            int score = scoreCounter.CurrentScore;
            bool isNewRecord = score > _highScores.Load();

            if (isNewRecord)
                _highScores.Save(score);

            GameSession.RegisterMatchResult(score, isNewRecord);
            StartCoroutine(LoadResultsAfterDelay());
        }

        private IEnumerator LoadResultsAfterDelay()
        {
            yield return new WaitForSeconds(resultsDelay);
            SceneManager.LoadScene(SceneNames.Results);
        }
    }
}