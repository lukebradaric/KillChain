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

        public event Action TargetDestroyed;

        private void TargetDestroyedHandler()
        {
            TargetDestroyed?.Invoke();
        }
    }
}

