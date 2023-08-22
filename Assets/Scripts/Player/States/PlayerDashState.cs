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
            _player.GameInput.SlamPressed += SlamPressedHandler;
            _player.GameInput.FireReleased += FireReleasedHandler;
            _player.Chain.TargetSetToNull += TargetSetToNullHandler;

            _player.CapsuleCollider.height = _player.Data.DashHeight;
        }

        public override void Exit()
        {
            _player.GameInput.SlamPressed -= SlamPressedHandler;
            _player.GameInput.FireReleased -= FireReleasedHandler;
            _player.Chain.TargetSetToNull -= TargetSetToNullHandler;

            _player.CapsuleCollider.height = _player.Data.Height;
        }

        public override void FixedUpdate()
        {
            _player.Rigidbody.SetVelocity((_player.Chain.Target.Transform.position - _stateMachine.transform.position).normalized * _player.Data.DashSpeed);

            if (Vector3.Distance(_stateMachine.transform.position, _player.Chain.Target.Transform.position) < _player.Data.DashStopDistance)
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
                    _player.Rigidbody.SetVelocity(_player.Rigidbody.velocity * _player.Data.DashBoostSpeedMultiplier);
                }
                else
                {
                    _player.Rigidbody.SetVelocity(-_player.Rigidbody.velocity.normalized * _player.Data.DashReboundSpeed);
                    _player.Rigidbody.SetVelocityY(_player.Data.DashDamageUpwardForce);
                }

                _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.IdleState);
                _stateMachine.ChangeState(_player.GroundCheck.IsFound() ? _stateMachine.MoveState : _stateMachine.AirState);
            }
        }

        private void SlamPressedHandler()
        {
            // If player is dashing while still on ground, return
            if (_player.GroundCheck.IsFound())
            {
                return;
            }

            _stateMachine.ChangeState(_stateMachine.SlamState);
        }

        private void FireReleasedHandler()
        {
            _player.Rigidbody.AddForce(Vector3.up * _player.Data.DashReleaseUpwardForce, ForceMode.Impulse);

            _stateMachine.ChangeState(_player.GroundCheck.IsFound() ? _stateMachine.MoveState : _stateMachine.AirState);
        }

        private void TargetSetToNullHandler()
        {
            _stateMachine.ChangeState(_player.GroundCheck.IsFound() ? _stateMachine.MoveState : _stateMachine.AirState);
        }
    }
}

