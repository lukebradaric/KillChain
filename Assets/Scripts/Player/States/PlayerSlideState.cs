using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerSlideState : PlayerState
    {
        public override void Enter()
        {
            _player.GameInput.SlideReleased += SlideReleasedHandler;
            _player.GameInput.JumpPressed += JumpPressedHandler;

            _player.CapsuleCollider.height = _player.Data.SlideHeight;
            _player.CapsuleCollider.material = _player.Data.SlidePhysicMaterial;
        }

        public override void Exit()
        {
            _player.GameInput.SlideReleased -= SlideReleasedHandler;
            _player.GameInput.JumpPressed -= JumpPressedHandler;

            _player.Rigidbody.drag = _player.Data.GroundDrag;
            _player.CapsuleCollider.height = _player.Data.Height;
            _player.CapsuleCollider.material = _player.Data.PhysicMaterial;
        }

        public override void FixedUpdate()
        {
            if (!_player.GroundCheck.IsFound())
            {
                _stateMachine.ChangeState(_stateMachine.AirState);
                return;
            }

            _player.Rigidbody.drag = _player.Data.SlideDrag.Evaluate(_player.Rigidbody.velocity.magnitude);

            // We aren't using flat velocity here because we know the player is grounded
            // Also, we want to make sure we include velocity for sliding down/up ramps
            // Also, don't cancel slide on angled surfaces because player will gain speed automatically
            if (_player.Rigidbody.velocity.magnitude < _player.Data.MinSlideSpeed && _player.GroundCheck.GetGroundAngle() < _player.Data.MinGroundAngle)
            {
                // TODO : Refactor this (consuming the slide pressed input)
                _player.GameInput.OnSlideReleased();

                _stateMachine.ChangeState(_stateMachine.MoveState);
                return;
            }

            if (_player.GroundCheck.GetGroundAngle() > _player.Data.MinGroundAngle)
            {
                _player.Rigidbody.AddForce(Vector3.down * _player.Data.AngledSlideDownwardsForce.Evaluate(_player.Rigidbody.velocity.magnitude));
            }
            else
            {
                _player.Rigidbody.AddForce(Vector3.down * _player.Data.SlideDownwardsForce);
            }
        }

        private void SlideReleasedHandler()
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }

        private void JumpPressedHandler()
        {
            this.Jump();
        }
    }
}

