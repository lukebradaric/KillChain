using System;
using UnityEngine;

namespace KillChain.Core.Events
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        public event Action<T> Event;
        public virtual void Invoke(T item) => Event?.Invoke(item);
    }
}

