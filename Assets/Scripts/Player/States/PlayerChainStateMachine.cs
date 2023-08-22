using KillChain.Core.StateMachine;
using UnityEngine;

namespace KillChain.Player.States
{
    public class PlayerChainStateMachine : StateMachine
    {
        [Space]
        [Header("Components")]
        [SerializeField] private Player _player;

        [Space]
        [Header("States")]
        [SerializeReference] private PlayerChainState _idleState;
        public PlayerChainState IdleState => _idleState;

        [SerializeReference] private PlayerChainState _throwDashState;
        public PlayerChainState ThrowDashState => _throwDashState;

        [SerializeReference] private PlayerChainState _throwPullState;
        public PlayerChainState ThrowPullState => _throwPullState;

        [SerializeReference] private PlayerChainState _dashState;
        public PlayerChainState DashState => _dashState;

        [SerializeReference] private PlayerChainState _pullState;
        public PlayerChainState PullState => _pullState;

        private void Start()
        {
            ChangeState(IdleState);
        }

        protected override void OnStateChanging(State newState)
        {
            ((PlayerChainState)newState)?.SetPlayer(_player);
        }
    }
}

