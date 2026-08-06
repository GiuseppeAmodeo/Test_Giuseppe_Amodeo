using System;
using UnityEngine;

namespace GemRush.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class Gem : MonoBehaviour
    {
        public static event Action<Gem> OnCollected;

        [Header("Idle animation")]
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private float bobAmplitude = 0.25f;
        [SerializeField] private float bobFrequency = 2f;

        private Vector3 _basePosition;
        private float _phase;
        private bool _collected;

        public void Initialize(Vector3 position)
        {
            transform.SetPositionAndRotation(position, Quaternion.identity);
            _basePosition = position;
            _phase = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            _collected = false;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            float y = _basePosition.y + Mathf.Sin(Time.time * bobFrequency + _phase) * bobAmplitude;
            transform.position = new Vector3(_basePosition.x, y, _basePosition.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_collected || !other.CompareTag("Player")) return;
            _collected = true; // guards against double-trigger in the same frame
            OnCollected?.Invoke(this);
        }
    }
}