using KillChain.Core;
using KillChain.Core.Extensions;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerSlamState : PlayerState
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameObject _slamParticlePrefab;

        public override void Enter()
        {
            // If player was dashing, change chain to idle state
            if (_stateMachine.PreviousState == _stateMachine.DashState)
            {
                _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.IdleState);
            }

            _player.Rigidbody.drag = _player.Data.AirDrag;
        }

        public override void Exit() { }

        public override void FixedUpdate()
        {
            base.Move(_player.Data.AirSpeedMultiplier);

            _player.Rigidbody.SetVelocityY(_player.Data.SlamSpeed);

            SlamDamage(_player.Data.SlamAirHitBoxSize);

            if (_player.GroundCheck.IsFound())
            {
                Slam();

                if (_player.JumpBuffer.Enabled)
                {
                    this.Jump();
                    _player.JumpBuffer.Consume();
                    return;
                }

                // If player is holding slam and flat velocity is valid, enter slide state
                if (_player.GameInput.IsSlidePressed && _player.Rigidbody.GetFlatVelocity().magnitude > _player.Data.MinSlideSpeed)
                {

                    _stateMachine.ChangeState(_stateMachine.SlideState);
                }
                else
                {
                    _stateMachine.ChangeState(_stateMachine.MoveState);
                }
            }
        }

        public override void OnDrawGizmos()
        {
            if (!_player)
            {
                return;
            }

            if (_player.GroundCheck.IsFound())
            {
                Gizmos.DrawWireCube(_player.SlamHitBoxTransform.position, _player.Data.SlamGroundHitBoxSize);
            }
            else
            {
                Gizmos.DrawWireCube(_player.SlamHitBoxTransform.position, _player.Data.SlamAirHitBoxSize);
            }

        }

        private void Slam()
        {
            _player.SlamEventChannel?.Invoke();
            GameObject.Instantiate(_slamParticlePrefab, _player.SlamHitBoxTransform.position, Quaternion.identity);
            SlamDamage(_player.Data.SlamGroundHitBoxSize);
        }

        private void SlamDamage(Vector3 slamHitBoxSize)
        {
            Collider[] colliders = Physics.OverlapBox(_player.SlamHitBoxTransform.position, slamHitBoxSize, Quaternion.identity, _player.Data.SlamLayerMask);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(_player.Data.SlamDamage);
                }
            }
        }
    }
}

