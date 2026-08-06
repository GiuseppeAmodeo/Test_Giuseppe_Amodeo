namespace GemRush.Core
{
    public interface IHighScoreRepository
    {
        int Load();
        void Save(int score);
    }
}