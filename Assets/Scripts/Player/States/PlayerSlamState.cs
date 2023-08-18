using KillChain.Core.Extensions;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerSlamState : PlayerState
    {
        public override void Enter()
        {
            _rigidbody.SetVelocityY(_playerData.SlamSpeed);
        }

        public override void Exit() { }

        public override void FixedUpdate()
        {
            Move();

            if (_playerGroundCheck.Found())
            {
                Debug.Log("Mock Slam Nearby Enemies!");
                _playerStateMachine.ChangeState(_playerStateMachine.MoveState);
            }
        }

        public override void Update() { }
    }
}

