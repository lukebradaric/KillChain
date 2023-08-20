using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerChainPullState : PlayerChainTargetState
    {
        public override void Enter()
        {
            base.Enter();
            _player.GameInput.AltFireReleased += AltFireReleasedHandler;
            _player.Chain.Target.StartPull(_player.transform, _player.Data.PullSpeed);
        }

        public override void Exit()
        {
            base.Exit();
            _player.GameInput.AltFireReleased -= AltFireReleasedHandler;

            // Stop pulling when exiting state
            _player.Chain.Target?.StopPull();
            _player.Chain.Target = null;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_player.Chain.Target != null && Vector3.Distance(_player.transform.position, _player.Chain.Target.Transform.position) < _player.Data.PullStopDistance)
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }

        private void AltFireReleasedHandler()
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}

