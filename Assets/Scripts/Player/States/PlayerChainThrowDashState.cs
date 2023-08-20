namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerChainThrowDashState : PlayerChainThrowState
    {
        public override void Enter()
        {
            base.Enter();
            _player.GameInput.FirePressed += FirePressedHandler;
            _player.GameInput.FireReleased += FireReleasedHandler;
        }

        public override void Exit()
        {
            base.Exit();
            _player.GameInput.FirePressed -= FirePressedHandler;
            _player.GameInput.FireReleased -= FireReleasedHandler;
        }

        protected override void ChangeState()
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }

        private void FirePressedHandler()
        {
            _cancelChange = false;
        }

        private void FireReleasedHandler()
        {
            _cancelChange = true;
        }
    }
}

