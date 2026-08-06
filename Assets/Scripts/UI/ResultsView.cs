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

        private void Start()
        {
            scoreText.text = $"Score: {GameSession.LastScore}";
            newRecordText.gameObject.SetActive(GameSession.IsNewRecord);

            menuButton.onClick.AddListener(() => SceneManager.LoadScene(SceneNames.Menu));
        }

        private void OnDestroy() => menuButton.onClick.RemoveAllListeners();
    }
}