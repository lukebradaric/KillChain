using KillChain.Core.Extensions;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerMoveState : PlayerState
    {
        public override void Enter()
        {
            _player.GameInput.JumpPressed += JumpPressedHandler;
            //_player.GameInput.FirePressed += FirePressedHandler;
            _player.GameInput.SlidePressed += SlidePressedHandler;

            _player.Rigidbody.drag = _player.Data.GroundDrag;
        }

        public override void Exit()
        {
            _player.GameInput.JumpPressed -= JumpPressedHandler;
            //_player.GameInput.FirePressed -= FirePressedHandler;
            _player.GameInput.SlidePressed -= SlidePressedHandler;
        }

        public override void FixedUpdate()
        {
            base.Move();

            if(!_player.GroundCheck.Found())
                _player.StateMachine.ChangeState(_player.StateMachine.AirState);
        }

        public override void Update() { }

        private void JumpPressedHandler()
        {
            _player.GroundCheck.TempDisable();
            _player.Rigidbody.SetVelocityY(_player.Data.JumpForce);
            _player.StateMachine.ChangeState(_player.StateMachine.AirState);
        }

        //private void FirePressedHandler()
        //{
        //    // If player left clicked while chained to enemy, enter dashing state
        //    if (_player.Weapon.State.Value != PlayerWeaponState.Attach)
        //        return;

        //    _player.StateMachine.ChangeState(_player.StateMachine.ThrowState);
        //}

        private void SlidePressedHandler()
        {
            _player.StateMachine.ChangeState(_player.StateMachine.SlideState);
        }
    }
}

