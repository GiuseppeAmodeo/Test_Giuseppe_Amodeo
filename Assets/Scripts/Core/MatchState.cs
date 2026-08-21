namespace GemRush.Core
{

    /// <summary>
    /// Unica fonte di verità sul ciclo di vita del match.
    /// Sostituisce i bool sparsi (IsRunning, _spawningEnabled,
    /// _inputEnabled) e il freeze di Time.timeScale.
    /// </summary>
    public enum MatchState
    {
        None,
        Countdown,
        Playing,
        Ended
    }

}