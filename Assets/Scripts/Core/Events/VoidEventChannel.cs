using System;
using UnityEngine;

namespace KillChain.Core.Events
{
    [CreateAssetMenu(menuName = "KillChain/EventChannels/VoidEventChannel")]
    public class VoidEventChannel : ScriptableObject
    {
        public event Action Event;
        public virtual void Invoke() => Event?.Invoke();
    }
}

