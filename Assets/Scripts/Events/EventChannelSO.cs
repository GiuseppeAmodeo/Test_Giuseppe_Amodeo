using System;
using UnityEngine;


namespace GemRush.Core.Events
{
    public abstract class EventChannelSO<T> : ScriptableObject
    {
        private event Action<T> Raised;

        public void Raise(T payload) => Raised?.Invoke(payload);
        public void Subscribe(Action<T> listener) => Raised += listener;
        public void Unsubscribe(Action<T> listener) => Raised -= listener;
    }

    public abstract class EventChannelSO : ScriptableObject
    {
        private event Action Raised;
        public void Raise() => Raised?.Invoke();
        public void Subscribe(Action listener) => Raised += listener;
        public void Unsubscribe(Action listener) => Raised -= listener;
    }
}
