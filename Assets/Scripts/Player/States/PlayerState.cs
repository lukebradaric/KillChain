using KillChain.Core.Extensions;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public abstract class PlayerState
    {
        public void SetPlayer(Player player) => _player = player;
        protected Player _player;

        public abstract void Enter();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void Exit();

        public virtual void OnDrawGizmos() { }

        protected Vector3 GetMoveDirection()
        {
            return (_player.LookTransform.forward * _player.GameInput.MoveInput.y + _player.LookTransform.right * _player.GameInput.MoveInput.x).normalized;
        }

        protected void Move(float velocityMultiplier = 1f)
        {
            Vector3 moveDirection = GetMoveDirection();

            // If ground angle greater than max, project movement direction on plane
            if (_player.GroundCheck.IsFound())
            {
                // If ground angle greater than max, don't move
                if (_player.GroundCheck.GetGroundAngle() > _player.Data.MaxGroundAngle)
                {
                    return;
                }
                // If ground angle greater than min, project movedirection on plane
                else if (_player.GroundCheck.GetGroundAngle() > _player.Data.MinGroundAngle)
                {
                    moveDirection = Vector3.ProjectOnPlane(moveDirection, _player.GroundCheck.GetGroundNormal());
                }
            }

            _player.Rigidbody.AddForce(moveDirection * _player.Data.MoveSpeed * velocityMultiplier);
        }

        protected void Jump()
        {
            _player.GroundCheck.Disable(_player.Data.JumpWaitTime);
            _player.Rigidbody.SetVelocityY(_player.Data.JumpForce);
            _player.StateMachine.ChangeState(_player.StateMachine.AirState);
        }
    }
}

