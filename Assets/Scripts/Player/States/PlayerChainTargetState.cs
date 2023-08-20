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

            // Chain Broke Event

            _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.IdleState);
        }

        private void TargetDestroyedHandler()
        {
            _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.IdleState);
        }
    }
}

