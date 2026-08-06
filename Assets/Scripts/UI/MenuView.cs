using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GemRush.Core;

namespace GemRush.UI
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private Button playButton;

        private void Start()
        {
            IHighScoreRepository highScores = new PlayerPrefsHighScoreRepository();
            int best = highScores.Load();
            highScoreText.text = best > 0 ? $"Best: {best}" : "No record yet";

            playButton.onClick.AddListener(() => SceneManager.LoadScene(SceneNames.Game));
        }

        private void OnDestroy() => playButton.onClick.RemoveAllListeners();
    }
}