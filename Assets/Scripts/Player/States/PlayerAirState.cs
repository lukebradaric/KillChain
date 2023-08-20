using System.Collections;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerAirState : PlayerState
    {
        public override void Enter()
        {
            _player.GameInput.SlamPressed += SlamPressedHandler;

            _player.Rigidbody.drag = _player.Data.AirDrag;
        }

        public override void Exit()
        {
            _player.GameInput.SlamPressed -= SlamPressedHandler;
        }

        public override void FixedUpdate()
        {
            base.Move(_player.Data.AirSpeedMultiplier);

            _player.Rigidbody.AddForce(Vector3.down * _player.Data.FallForce);

            // If no ground check found, return
            if (!_player.GroundCheck.IsFound())
            {
                return;
            }

            if (_player.JumpBuffer.Enabled)
            {
                this.Jump();
                _player.JumpBuffer.Consume();
                return;
            }

            // If ground check and no jump buffer, enter move state
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }

        private void SlamPressedHandler()
        {
            _stateMachine.ChangeState(_stateMachine.SlamState);
        }
    }
}

