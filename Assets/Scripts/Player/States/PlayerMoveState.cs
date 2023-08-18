using KillChain.Core.Extensions;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerMoveState : PlayerState
    {
        public override void Enter()
        {
            _gameInput.JumpPressed += JumpPressedHandler;
            _gameInput.FirePressed += FirePressedHandler;
            _gameInput.SlidePressed += SlidePressedHandler;

            _rigidbody.drag = _playerData.GroundDrag;
        }

        public override void Exit()
        {
            _gameInput.JumpPressed -= JumpPressedHandler;
            _gameInput.FirePressed -= FirePressedHandler;
            _gameInput.SlidePressed -= SlidePressedHandler;
        }

        public override void FixedUpdate()
        {
            Move();

            if(!_playerGroundCheck.Found())
                _playerStateMachine.ChangeState(_playerStateMachine.AirState);
        }

        public override void Update() { }

        private void JumpPressedHandler()
        {
            _playerGroundCheck.TempDisable();
            _rigidbody.SetVelocityY(_playerData.JumpForce);
            _playerStateMachine.ChangeState(_playerStateMachine.AirState);
        }

        private void FirePressedHandler()
        {
            // If player left clicked while chained to enemy, enter dashing state
            if (_playerWeapon.State.Value != PlayerWeaponState.Attach)
                return;

            _playerStateMachine.ChangeState(_playerStateMachine.DashState);
        }

        private void SlidePressedHandler()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.SlideState);
        }
    }
}

