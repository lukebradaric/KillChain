using System;
using UnityEngine;

namespace KillChain.Player.States
{
    public class PlayerStateMachine : PlayerMonoBehaviour
    {
        // TODO : Refactor component references and passing to other substates

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

        [Space]
        [Header("DEBUG")]
        // TODO : Remove [SerializeField]
        [SerializeReference] private PlayerState _currentState;

        public Type CurrentStateType => _currentState.GetType();

        private void Start()
        {
            ChangeState(_moveState);
        }

        public void ChangeState(PlayerState _newState)
        {
            _currentState?.Exit();
            _currentState = _newState;
            _currentState?.SetPlayer(_player);
            _currentState?.Enter();
        }

        private void Update()
        {
            _currentState?.Update();
        }

        private void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        private void OnDestroy()
        {
            _currentState.Exit();
        }
    }
}

