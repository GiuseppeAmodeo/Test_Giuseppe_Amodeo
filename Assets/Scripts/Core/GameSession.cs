namespace GemRush.Core
{
    /// <summary>
    /// Holds transient data that must survive scene loads within a single app run.
    /// Kept static on purpose: no scene object lifetime to manage for two ints.
    /// </summary>
    public static class GameSession
    {
        public static int LastScore { get; private set; }
        public static bool IsNewRecord { get; private set; }

        public static void RegisterMatchResult(int score, bool isNewRecord)
        {
            LastScore = score;
            IsNewRecord = isNewRecord;
        }
    }
}