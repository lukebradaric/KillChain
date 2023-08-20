using KillChain.Core;
using KillChain.Core.Events;
using KillChain.Core.Extensions;
using UnityEngine;

namespace KillChain.Player.States
{
    [System.Serializable]
    public class PlayerSlamState : PlayerState
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerSlamEventChannel;

        [Space]
        [Header("Components")]
        [SerializeField] private GameObject _slamParticlePrefab;
        [SerializeField] private LayerMask _slamLayerMask;

        public override void Enter()
        {
            // If player was dashing, change chain to idle state
            if (_player.StateMachine.PreviousState == _player.StateMachine.DashState)
            {
                _player.ChainStateMachine.ChangeState(_player.ChainStateMachine.IdleState);
            }
        }

        public override void Exit() { }

        public override void FixedUpdate()
        {
            base.Move();

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

                _player.StateMachine.ChangeState(_player.StateMachine.MoveState);
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
            _playerSlamEventChannel?.Invoke();
            GameObject.Instantiate(_slamParticlePrefab, _player.SlamHitBoxTransform.position, Quaternion.identity);
            SlamDamage(_player.Data.SlamGroundHitBoxSize);
        }

        private void SlamDamage(Vector3 slamHitBoxSize)
        {
            Collider[] colliders = Physics.OverlapBox(_player.SlamHitBoxTransform.position, slamHitBoxSize, Quaternion.identity, _slamLayerMask);
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

