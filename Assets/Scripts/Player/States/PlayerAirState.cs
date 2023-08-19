using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerAirState : PlayerState
    {
        public override void Enter()
        {
            //_player.GameInput.FirePressed += FirePressedHandler;
            _player.GameInput.SlamPressed += SlamPressedHandler;

            _player.Rigidbody.drag = _player.Data.AirDrag;
        }

        public override void Exit()
        {
            //_player.GameInput.FirePressed -= FirePressedHandler;
            _player.GameInput.SlamPressed -= SlamPressedHandler;
        }

        public override void FixedUpdate()
        {
            base.Move(_player.Data.AirSpeedMultiplier);

            _player.Rigidbody.AddForce(Vector3.down * _player.Data.FallForce);

            if (_player.GroundCheck.Found())
            {
                _player.StateMachine.ChangeState(_player.StateMachine.MoveState);
            }
        }

        public override void Update() { }

        //private void FirePressedHandler()
        //{
        //    // If player left clicked while chained to enemy, enter dashing state
        //    if (_player.Weapon.State.Value != PlayerWeaponState.Attach)
        //        return;

        //    _player.StateMachine.ChangeState(_player.StateMachine.ThrowState);
        //}

        protected virtual void SlamPressedHandler()
        {
            _player.StateMachine.ChangeState(_player.StateMachine.SlamState);
        }
    }
}

