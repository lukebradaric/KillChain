using KillChain.Core.Extensions;

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

            if(!_player.GroundCheck.IsFound())
                _player.StateMachine.ChangeState(_player.StateMachine.AirState);
        }

        public override void Update() { }

        private void JumpPressedHandler()
        {
            this.Jump();
        }

        private void SlidePressedHandler()
        {
            _player.StateMachine.ChangeState(_player.StateMachine.SlideState);
        }
    }
}

