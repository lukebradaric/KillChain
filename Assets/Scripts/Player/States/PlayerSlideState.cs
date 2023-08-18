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
            _slideDirection = _player.LookTransform.forward;
            _slideDirection.y = 0;

            _player.GameInput.SlideReleased += SlideReleasedHandler;
        }

        public override void Exit()
        {
            _player.GameInput.SlideReleased -= SlideReleasedHandler;
        }

        public override void FixedUpdate()
        {
            Vector3 slideVelocity = _slideDirection * _player.Data.SlideSpeed;
            _player.Rigidbody.SetVelocity(slideVelocity.x, _player.Rigidbody.velocity.y, slideVelocity.z);
        }

        public override void Update() { }

        private void SlideReleasedHandler()
        {
            _player.StateMachine.ChangeState(_player.StateMachine.MoveState);
        }
    }
}

