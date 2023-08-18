using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerAirState : PlayerState
    {
        public override void Enter()
        {
            _gameInput.FirePressed += FirePressedHandler;
            _gameInput.SlamPressed += SlamPressedHandler;

            _rigidbody.drag = _playerData.AirDrag;
        }

        public override void Exit()
        {
            _gameInput.FirePressed -= FirePressedHandler;
            _gameInput.SlamPressed -= SlamPressedHandler;
        }

        public override void FixedUpdate()
        {
            Move(_playerData.AirSpeedMultiplier);

            LimitVelocity(_playerData.MaxAirSpeed);

            if (_playerGroundCheck.Found())
            {
                _playerStateMachine.ChangeState(_playerStateMachine.MoveState);
            }
        }

        public override void Update() { }

        private void FirePressedHandler()
        {
            // If player left clicked while chained to enemy, enter dashing state
            if (_playerWeapon.State.Value != PlayerWeaponState.Attach)
                return;

            _playerStateMachine.ChangeState(_playerStateMachine.DashState);
        }

        protected virtual void SlamPressedHandler()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.SlamState);
        }
    }
}

