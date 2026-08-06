using UnityEngine;

namespace GemRush.Core
{
    /// <summary>
    /// PlayerPrefs-backed persistence. Swappable (e.g. JSON file, cloud save)
    /// without touching gameplay code.
    /// </summary>
    public class PlayerPrefsHighScoreRepository : IHighScoreRepository
    {
        private const string Key = "gemrush.highscore";

        public int Load() => PlayerPrefs.GetInt(Key, 0);

        public void Save(int score)
        {
            PlayerPrefs.SetInt(Key, score);
            PlayerPrefs.Save();
        }
    }
}