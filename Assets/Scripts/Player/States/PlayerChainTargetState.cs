namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerChainTargetState : PlayerChainState
    {
        public override void Enter()
        {
            _player.Chain.TargetSetToNull += TargetSetToNullHandler;
        }

        public override void Exit()
        {
            _player.Chain.TargetSetToNull -= TargetSetToNullHandler;
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

        private void TargetSetToNullHandler()
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}

