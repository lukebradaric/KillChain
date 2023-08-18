using KillChain.Core.Extensions;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerSlideState : PlayerState
    {
        Vector3 _slideDirection;

        public override void Enter()
        {
            _slideDirection = _lookTransform.forward;
            _slideDirection.y = 0;

            _gameInput.SlideReleased += SlideReleasedHandler;
        }

        public override void Exit()
        {
            _gameInput.SlideReleased -= SlideReleasedHandler;
        }

        public override void FixedUpdate()
        {
            Vector3 slideVelocity = _slideDirection * _playerData.SlideSpeed;
            _rigidbody.SetVelocity(slideVelocity.x, _rigidbody.velocity.y, slideVelocity.z);
        }

        public override void Update() { }

        private void SlideReleasedHandler()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.MoveState);
        }
    }
}

