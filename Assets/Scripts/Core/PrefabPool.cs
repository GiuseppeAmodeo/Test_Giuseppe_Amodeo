using UnityEngine;
using UnityEngine.Pool;

namespace GemRush.Core
{
    /// <summary>
    /// Thin generic wrapper around UnityEngine.Pool.ObjectPool for
    /// prefab-based components (gems, particle bursts, ...).
    /// </summary>
    public class PrefabPool<T> where T : Component
    {
        private readonly ObjectPool<T> _pool;

        public PrefabPool(T prefab, Transform parent, int defaultCapacity = 8)
        {
            _pool = new ObjectPool<T>(
                createFunc: () => Object.Instantiate(prefab, parent),
                actionOnGet: item => item.gameObject.SetActive(true),
                actionOnRelease: item => item.gameObject.SetActive(false),
                actionOnDestroy: item => Object.Destroy(item.gameObject),
                defaultCapacity: defaultCapacity);
        }

        public T Get() => _pool.Get();
        public void Release(T item) => _pool.Release(item);
    }
}