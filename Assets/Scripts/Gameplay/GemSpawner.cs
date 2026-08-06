using System.Collections.Generic;
using UnityEngine;
using GemRush.Core;

namespace GemRush.Gameplay
{
    public class GemSpawner : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private Gem gemPrefab;
        [SerializeField] private float spawnHeight = 0.75f;
        [SerializeField] private float edgePadding = 0.5f;

        private PrefabPool<Gem> _gemPool;
        private readonly List<Gem> _activeGems = new();
        private float _timer;
        private bool _spawningEnabled = true;

        private void Awake()
        {
            _gemPool = new PrefabPool<Gem>(gemPrefab, transform, config.maxConcurrentGems);
        }

        private void OnEnable()
        {
            Gem.OnCollected += HandleGemCollected;
            MatchTimer.OnMatchEnded += StopSpawning;
        }

        private void OnDisable()
        {
            Gem.OnCollected -= HandleGemCollected;
            MatchTimer.OnMatchEnded -= StopSpawning;
        }

        private void Update()
        {
            if (!_spawningEnabled || _activeGems.Count >= config.maxConcurrentGems) return;

            _timer += Time.deltaTime;
            if (_timer < config.spawnInterval) return;

            _timer = 0f;
            SpawnGem();
        }

        private void SpawnGem()
        {
            Vector2 extents = config.arenaHalfExtents - Vector2.one * edgePadding;
            Vector3 position = new(
                Random.Range(-extents.x, extents.x),
                spawnHeight,
                Random.Range(-extents.y, extents.y));

            Gem gem = _gemPool.Get();
            gem.Initialize(position);
            _activeGems.Add(gem);
        }

        private void HandleGemCollected(Gem gem)
        {
            _activeGems.Remove(gem);
            _gemPool.Release(gem);
        }
    

        private void StopSpawning()
        {
            _spawningEnabled = false;
            foreach (Gem gem in _activeGems)
                _gemPool.Release(gem);
            _activeGems.Clear();
        }
    }
}