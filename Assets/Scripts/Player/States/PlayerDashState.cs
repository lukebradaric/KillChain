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
            _player.GameInput.FirePressed += FirePressedHandler;
            _player.GameInput.SlamPressed += SlamPressedHandler;
        }

        public override void Exit()
        {
            _player.GameInput.FirePressed -= FirePressedHandler;
            _player.GameInput.SlamPressed -= SlamPressedHandler;
        }

        public override void FixedUpdate()
        {
            _player.Rigidbody.SetVelocity((_player.Chain.Target.Transform.position - _player.StateMachine.transform.position).normalized * _player.Data.DashSpeed);

            if (Vector3.Distance(_player.StateMachine.transform.position, _player.Chain.Target.Transform.position) < _player.Data.DashStopDistance)
            {
                if (_player.Chain.Target.Transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(_player.Data.DashDamage);
                    _player.Rigidbody.SetVelocity(_player.Rigidbody.velocity * _player.Data.DashDamageSpeedMultiplier);
                    _player.Rigidbody.SetVelocityY(_player.Data.DashDamageUpwardForce);
                }
                else if (_player.Chain.Target.IsBoostable)
                {
                    // Boost Event
                    _player.Rigidbody.SetVelocity(_player.Rigidbody.velocity * _player.Data.BoostSpeedMultiplier);
                }
                else
                {
                    _player.Rigidbody.SetVelocity(-_player.Rigidbody.velocity.normalized * _player.Data.DashReboundSpeed);
                    _player.Rigidbody.SetVelocityY(_player.Data.DashDamageUpwardForce);
                }

                //  TODO : Invoke event here instead of directly setting chain target and state?
                _player.Chain.ForceIdleState();
                _player.StateMachine.ChangeState(_player.StateMachine.AirState);
            }
        }

        public override void Update() { }

        private void FirePressedHandler()
        {
            _player.StateMachine.ChangeState(_player.StateMachine.AirState);
        }

        private void SlamPressedHandler()
        {
            // If player is dashing while still on ground, return
            if (_player.GroundCheck.Found())
            {
                return;
            }

            _player.StateMachine.ChangeState(_player.StateMachine.SlamState);
        }
    }
}

