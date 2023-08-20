using KillChain.Core;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerChainIdleState : PlayerChainState
    {
        private IChainTarget _lookTarget;

        public override void Enter()
        {
            _player.GameInput.FirePressed += FirePressedHandler;
            _player.GameInput.AltFirePressed += AltFirePressedHandler;
            _player.Chain.Target = null;
        }

        public override void Exit()
        {
            _player.GameInput.FirePressed -= FirePressedHandler;
            _player.GameInput.AltFirePressed -= AltFirePressedHandler;
        }

        public override void FixedUpdate()
        {
            UpdateLookTarget();
        }

        private void FirePressedHandler()
        {
            if (_lookTarget == null)
            {
                return;
            }

            if (!_lookTarget.IsDashable)
            {
                return;
            }

            if (!this.IsTargetInLineOfSight(_lookTarget))
            {
                return;
            }

            _player.Chain.Target = _lookTarget;
            _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.ThrowDashState);
        }

        private void AltFirePressedHandler()
        {
            if (_lookTarget == null)
            {
                return;
            }

            if (!_lookTarget.IsPullable)
            {
                return;
            }

            if (!this.IsTargetInLineOfSight(_lookTarget))
            {
                return;
            }

            _player.Chain.Target = _lookTarget;
            _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.ThrowPullState);
        }

        private void UpdateLookTarget()
        {
            _lookTarget = this.GetLookTarget();
        }
    }
}

