using KillChain.Core;
using System;

namespace KillChain.Player
{
    public class PlayerChain : PlayerMonoBehaviour
    {
        private IChainTarget _target;
        public IChainTarget Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (_target != null)
                {
                    // Unsubscribe from previous target if not null
                    _target.Destroyed -= TargetDestroyedHandler;
                }

                _target = value;

                if (_target == null)
                {
                    // If new target null, call event
                    TargetSetToNull?.Invoke();
                }

                if (_target != null)
                {
                    // Subscribe to new target if not null
                    _target.Destroyed += TargetDestroyedHandler;
                }
            }
        }

        //public event Action TargetDestroyed;
        public event Action TargetSetToNull;

        private void TargetDestroyedHandler()
        {
            Target = null;
        }
    }
}

