using System.Collections;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public abstract class PlayerChainThrowState : PlayerChainTargetState
    {
        private Coroutine _delayCoroutine;
        protected bool _cancelChange = false;

        public override void Enter()
        {
            base.Enter();
            _delayCoroutine = _player.StartCoroutine(DelayCoroutine());
            _player.ChainThrowEventChannel?.Invoke();
        }

        public override void Exit()
        {
            base.Exit();
            _cancelChange = false;
            _player.StopCoroutine(_delayCoroutine);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected abstract void ChangeState();

        private IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(CalculateChangeStateDelay());

            if (_cancelChange)
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
                yield break;
            }

            _player.ChainHitEventChannel?.Invoke();
            ChangeState();
        }

        private float CalculateChangeStateDelay()
        {
            return _player.Data.MaxChainDelayTime / (_player.Data.MaxChainDistance / Vector3.Distance(_player.transform.position, _player.Chain.Target.Transform.position));
        }
    }
}

