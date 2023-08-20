namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerChainThrowPullState : PlayerChainThrowState
    {
        public override void Enter()
        {
            base.Enter();
            _player.GameInput.AltFirePressed += AltFirePressedHandler;
            _player.GameInput.AltFireReleased += AltFireReleasedHandler;
        }

        public override void Exit()
        {
            base.Exit();
            _player.GameInput.AltFirePressed -= AltFirePressedHandler;
            _player.GameInput.AltFireReleased -= AltFireReleasedHandler;
        }

        protected override void ChangeState()
        {
            _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.PullState);
        }

        private void AltFirePressedHandler()
        {
            _cancelChange = false;
        }

        private void AltFireReleasedHandler()
        {
            _cancelChange = true;
        }
    }
}

