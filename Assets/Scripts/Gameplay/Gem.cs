using System;
using UnityEngine;

namespace GemRush.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class Gem : MonoBehaviour
    {
        //public static event Action<Gem> OnCollected;

        [Header("Broadcasts On")]
        [SerializeField] private Core.Events.EventChannelSO<Gem> collectedChannel;

        [Header("Visual child transform no collider")]
        [SerializeField] private Transform visual;

        [Header("Idle animation")]
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private float bobAmplitude = 0.25f;
        [SerializeField] private float bobFrequency = 2f;

        private float _phase;
        private bool _collected;

        public void Initialize(Vector3 position)
        {
            transform.SetPositionAndRotation(position, Quaternion.identity);
            _phase = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            _collected = false;
            visual.localPosition = Vector3.zero;
        }

        private void Update()
        {
            visual.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            float y = Mathf.Sin(Time.time * bobFrequency + _phase) * bobAmplitude;
            visual.localPosition = new Vector3(0f,y,0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_collected || !other.CompareTag("Player")) return;
            _collected = true; // guards against double-trigger in the same frame
            collectedChannel.Raise(this);

            //OnCollected?.Invoke(this);
        }
    }
}