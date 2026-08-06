using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GemRush.Core;

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

        private readonly IHighScoreRepository _highScores = new PlayerPrefsHighScoreRepository();

        private void OnEnable() => MatchTimer.OnMatchEnded += HandleMatchEnded;
        private void OnDisable() => MatchTimer.OnMatchEnded -= HandleMatchEnded;

        private void HandleMatchEnded()
        {
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