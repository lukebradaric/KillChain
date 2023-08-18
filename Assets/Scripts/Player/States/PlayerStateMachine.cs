using KillChain.Input;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerStateMachine : MonoBehaviour
    {
        [Space]
        [Header("Entry State")]
        [SerializeReference] private PlayerState _entryState;

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
        [Header("Components")]
        public GameInput _gameInput;
        public PlayerData _playerData;
        public Rigidbody _rigidbody;
        public Transform _lookTransform;
        public PlayerWeapon _playerWeapon;
        public PlayerGroundCheck _playerGroundCheck;

        [Space]
        [Header("DEBUG")]
        // TODO : Remove [SerializeField]
        [SerializeReference] private PlayerState _currentState;

        private void Start()
        {
            ChangeState(_entryState);
        }

        public void ChangeState(PlayerState _newState)
        {
            _currentState?.Exit();
            _currentState = _newState;

            // debug pass vars
            _currentState._gameInput = _gameInput;
            _currentState._playerData = _playerData;
            _currentState._rigidbody = _rigidbody;
            _currentState._lookTransform = _lookTransform;
            _currentState._playerWeapon = _playerWeapon;
            _currentState._playerStateMachine = this;
            _currentState._playerGroundCheck = _playerGroundCheck;

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

