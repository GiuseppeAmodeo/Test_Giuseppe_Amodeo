using UnityEngine;


namespace GemRush.Core
{

    [CreateAssetMenu(fileName = "MatchResultSO", menuName = "GemRush/Match Result")]
    public class MatchResultSO : ScriptableObject
    {
        public int LastScore { get; private set; }
        public bool IsNewRecord { get; private set; }

        public void Register(int score, bool isNewRecord)
        {
            LastScore = score;
            IsNewRecord = isNewRecord;
        }

        public void Clear () => Register(0, false); 

    }
}
