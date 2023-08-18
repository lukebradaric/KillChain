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

            if (Vector3.Distance(_playerStateMachine.transform.position, _playerWeapon.CurrentChainable.Transform.position) < _playerData.DashStopDistance)
            {
                Debug.Log("Mock Damage Dash Target!");
                if (_playerWeapon.CurrentChainable.Transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(_playerData.DashDamage);
                    _rigidbody.SetVelocity(_rigidbody.velocity * _playerData.DashDamageSpeedMultiplier);
                    _rigidbody.SetVelocityY(_playerData.DashDamageUpwardForce);
                }
                //else if( THE CHAINABLE IS A BOOSTABLE )
                else
                {
                    //_rigidbody.SetVelocity(_rigidbody.velocity * _playerData.BoostSpeedMultiplier);
                    _rigidbody.SetVelocity(-_rigidbody.velocity.normalized * _playerData.DashReboundSpeed);
                    _rigidbody.SetVelocityY(_playerData.DashDamageUpwardForce);
                }

                _playerStateMachine.ChangeState(_playerStateMachine.AirState);
                _playerWeapon.State.Value = PlayerWeaponState.Idle;
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

