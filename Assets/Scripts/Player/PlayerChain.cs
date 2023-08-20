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
                // Unsubscribe old target
                if (_target != null)
                {
                    _target.Destroyed -= TargetDestroyedHandler;
                }

                _target = value;

                // Subscribe new target
                if (_target != null)
                {
                    _target.Destroyed += TargetDestroyedHandler;
                }
            }
        }

        // TODO : Improve debuggign
        public string debugTarget;
        private void Update()
        {
            if (_target == null)
            {
                debugTarget = "null";
                return;
            }
            debugTarget = _target.Transform.name;
        }

        public event Action TargetDestroyed;

        private void TargetDestroyedHandler()
        {
            TargetDestroyed?.Invoke();
        }
    }
}

