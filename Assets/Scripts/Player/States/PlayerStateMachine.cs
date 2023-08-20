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
        [SerializeField] private string _currentStateName;

        public PlayerState CurrentState { get; private set; }

        private void Start()
        {
            ChangeState(_moveState);
        }

        public void ChangeState(PlayerState _newState)
        {
            CurrentState?.Exit();
            CurrentState = _newState;
            CurrentState?.SetPlayer(_player);
            CurrentState?.Enter();
        }

        private void Update()
        {
            CurrentState?.Update();

            _currentStateName = CurrentState.GetType().Name;
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        private void OnDrawGizmos()
        {
            _moveState.OnDrawGizmos();
            _airState.OnDrawGizmos();
            _dashState.OnDrawGizmos();
            _slamState.OnDrawGizmos();
            _slideState.OnDrawGizmos();
        }

        private void OnDestroy()
        {
            CurrentState.Exit();
        }
    }
}

