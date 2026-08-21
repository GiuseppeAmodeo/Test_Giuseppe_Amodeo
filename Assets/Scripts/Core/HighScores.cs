using UnityEngine;

namespace GemRush.Core
{
    /// <summary>
    /// Single composition point for high-score persistence: the only place in the
    /// project that knows which <see cref="IHighScoreRepository"/> implementation is in use.
    /// Swapping to a JSON or cloud backend is a one-line change here, with no
    /// gameplay or UI code touched.
    /// </summary>
    public static class HighScores
    {
        private static IHighScoreRepository _repository;

        public static IHighScoreRepository Repository
            => _repository ??= new PlayerPrefsHighScoreRepository();

        /// <summary>Injection seam for tests and for platform-specific backends.</summary>
        public static void UseRepository(IHighScoreRepository repository) => _repository = repository;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => _repository = null;
    }
}
