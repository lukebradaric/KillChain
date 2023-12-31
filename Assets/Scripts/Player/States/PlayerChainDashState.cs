﻿namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerChainDashState : PlayerChainTargetState
    {
        public override void Enter()
        {
            base.Enter();
            _player.StateMachine.ChangeState(_player.StateMachine.DashState);
            _player.GameInput.FireReleased += FireReleasedHandler;
        }

        public override void Exit()
        {
            base.Exit();
            _player.GameInput.FireReleased -= FireReleasedHandler;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void FireReleasedHandler()
        {
            _player.Chain.Target = null;

            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}

