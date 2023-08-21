namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerChainTargetState : PlayerChainState
    {
        public override void Enter()
        {
            _player.Chain.TargetDestroyed += TargetDestroyedHandler;
        }

        public override void Exit()
        {
            _player.Chain.TargetDestroyed -= TargetDestroyedHandler;
        }

        public override void FixedUpdate()
        {
            if (this.IsTargetInLineOfSight(_player.Chain.Target))
            {
                return;
            }

            _player.ChainBreakEventChannel?.Invoke();
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }

        private void TargetDestroyedHandler()
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}

