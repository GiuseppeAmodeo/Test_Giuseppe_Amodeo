using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GemRush.Core;

namespace GemRush.UI
{
    public class ResultsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text newRecordText;
        [SerializeField] private Button menuButton;

        [SerializeField] private MatchResultSO matchResult;

        private void Start()
        {
            scoreText.text = $"Score: {matchResult.LastScore}";
            newRecordText.gameObject.SetActive(matchResult.IsNewRecord);

            menuButton.onClick.AddListener(() => SceneManager.LoadScene(SceneNames.Menu));
        }

        private void OnDestroy() => menuButton.onClick.RemoveAllListeners();
    }
}