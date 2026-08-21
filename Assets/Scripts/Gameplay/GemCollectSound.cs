using System;
using UnityEngine;


namespace GemRush.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class GemCollectSound : MonoBehaviour
    {
        [Header("Listens To")]
        [SerializeField] private GemEventChannelSO gemCollectedChannel;

        [Header("Sounds")]
        [SerializeField] private AudioClip collectClip;
        [SerializeField, Range(0f, 0.3f)] private float pitchVariation = 0.1f;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            gemCollectedChannel.Subscribe(HandleGemCollected);
        }

        private void OnDisable()
        {
            gemCollectedChannel.Unsubscribe(HandleGemCollected);
        }

        private void HandleGemCollected(Gem gem)
        {
            audioSource.pitch = 1f + UnityEngine.Random.Range(-pitchVariation, pitchVariation);
            audioSource.PlayOneShot(collectClip);
        }
    }
}
