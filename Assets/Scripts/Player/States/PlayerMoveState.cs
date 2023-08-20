using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerMoveState : PlayerState
    {

        public override void Enter()
        {
            _player.GameInput.JumpPressed += JumpPressedHandler;
            _player.GameInput.SlidePressed += SlidePressedHandler;

            _player.Rigidbody.drag = _player.Data.GroundDrag;
        }

        public override void Exit()
        {
            _player.GameInput.JumpPressed -= JumpPressedHandler;
            _player.GameInput.SlidePressed -= SlidePressedHandler;
        }

        public override void FixedUpdate()
        {
            base.Move();

            if (!_player.GroundCheck.IsFound())
            {
                _stateMachine.ChangeState(_stateMachine.AirState);
                return;
            }

            // If ground angle greater than max, apply downwards force
            if(_player.GroundCheck.GetGroundAngle() > _player.Data.MaxGroundAngle)
            {
                _player.Rigidbody.AddForce(Vector3.down * _player.Data.DownwardsForce);
                return;
            }

            // If ground angle greater than min, apply force against plane
            if (_player.GroundCheck.GetGroundAngle() > _player.Data.MinGroundAngle)
            {
                _player.Rigidbody.AddForce(-_player.GroundCheck.GetGroundNormal() * _player.Data.DownwardsForce);
                return;
            }

            // Apply regular downwards force
            _player.Rigidbody.AddForce(Vector3.down * _player.Data.DownwardsForce);
        }

        private void JumpPressedHandler()
        {
            this.Jump();
        }

        private void SlidePressedHandler()
        {
            _stateMachine.ChangeState(_stateMachine.SlideState);
        }
    }
}

