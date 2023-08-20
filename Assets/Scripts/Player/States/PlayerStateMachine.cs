using KillChain.Core.StateMachine;
using UnityEngine;

namespace KillChain.Player.States
{
    public class PlayerStateMachine : StateMachine
    {
        [Space]
        [Header("Components")]
        [SerializeField] private Player _player;

        [Space]
        [Header("States")]
        [SerializeReference] private PlayerState _moveState;
        public PlayerState MoveState => _moveState;
        [SerializeReference] private PlayerState _airState;
        public PlayerState AirState => _airState;
        [SerializeReference] private PlayerState _dashState;
        public PlayerState DashState => _dashState;
        [SerializeReference] private PlayerState _slamState;
        public PlayerState SlamState => _slamState;
        [SerializeReference] private PlayerState _slideState;
        public PlayerState SlideState => _slideState;

        private void Start()
        {
            ChangeState(_moveState);
        }

        protected override void OnStateChanging(State newState)
        {
            ((PlayerState)newState)?.SetPlayer(_player);
        }

    }
}

