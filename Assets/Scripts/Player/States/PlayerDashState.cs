using KillChain.Core;
using KillChain.Core.Extensions;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerDashState : PlayerState
    {
        public override void Enter()
        {
            _gameInput.FirePressed += FirePressedHandler;
            _gameInput.SlamPressed += SlamPressedHandler;
        }

        public override void Exit()
        {
            _gameInput.FirePressed -= FirePressedHandler;
            _gameInput.SlamPressed -= SlamPressedHandler;
        }

        public override void FixedUpdate()
        {
            _rigidbody.SetVelocity((_playerWeapon.CurrentChainable.Transform.position - _playerStateMachine.transform.position).normalized * _playerData.DashSpeed);

            if (Vector3.Distance(_playerStateMachine.transform.position, _playerWeapon.CurrentChainable.Transform.position) < _playerData.DashDamageDistance)
            {
                Debug.Log("Mock Damage Dash Target!");
                if (_playerWeapon.CurrentChainable.Transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(_playerData.DashDamage);
                    _rigidbody.SetVelocityY(_playerData.DashDamageUpwardForce);
                }
                else
                {
                    _rigidbody.SetVelocity(-_rigidbody.velocity.normalized * _playerData.DashReboundSpeed);
                    _rigidbody.SetVelocityY(_playerData.DashDamageUpwardForce);

                    _playerStateMachine.ChangeState(_playerStateMachine.AirState);
                }
            }
        }

        public override void Update() { }

        private void FirePressedHandler()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.AirState);
        }

        private void SlamPressedHandler()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.SlamState);
        }
    }
}

