using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KillChain.Core.Events
{
    [System.Serializable]
    internal struct VoidEventListenerData
    {
        [SerializeField] private VoidEventChannel _eventChannel;
        [SerializeField] private UnityEvent _unityEvent;

        public void Enable()
        {
            _eventChannel.Event += EventHandler;
        }

        public void Disable()
        {
            _eventChannel.Event -= EventHandler;
        }

        private void EventHandler()
        {
            _unityEvent?.Invoke();
        }
    }

    [System.Serializable]
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] private List<VoidEventListenerData> _eventListeners = new List<VoidEventListenerData>();

        private void OnEnable()
        {
            foreach (var listener in _eventListeners)
            {
                listener.Enable();
            }
        }

        private void OnDisable()
        {
            foreach (var listener in _eventListeners)
            {
                listener.Disable();
            }
        }
    }
}

