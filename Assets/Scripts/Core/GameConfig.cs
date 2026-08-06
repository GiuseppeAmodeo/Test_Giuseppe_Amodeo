using UnityEngine;


namespace GemRush.Core
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "GemRush/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Match")]
        [Min(5f)] public float matchDuration = 60f;

        [Header("Arena")]
        [Tooltip("Half extents of the playable area on X/Z, centered at origin.")]
        public Vector2 arenaHalfExtents = new(9f, 9f);

        [Header("Gems")]
        [Min(0.1f)] public float spawnInterval = 1.5f;
        [Min(1)] public int maxConcurrentGems = 6;
        [Min(1)] public int gemValue = 1;

        [Header("Player")]
        [Min(0.5f)] public float moveSpeed = 7f;
    }   
}
