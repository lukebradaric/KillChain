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
        [SerializeField] private Transform _slamHitboxTransform;
        [SerializeField] private GameObject _slamParticlePrefab;
        [SerializeField] private LayerMask _slamLayerMask;

        public override void Enter() { }

        public override void Exit() { }

        public override void FixedUpdate()
        {
            base.Move();

            _player.Rigidbody.SetVelocityY(_player.Data.SlamSpeed);

            if (_player.GroundCheck.Found())
            {
                Slam();

                _player.StateMachine.ChangeState(_player.StateMachine.MoveState);
            }
        }

        public override void Update() { }

        public override void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_slamHitboxTransform.position, new Vector3(5, 1, 5));
        }

        private void Slam()
        {
            _playerSlamEventChannel?.Invoke();
            //IsSlamming.Value = false;
            GameObject.Instantiate(_slamParticlePrefab, _slamHitboxTransform.position, Quaternion.identity);

            Collider[] colliders = Physics.OverlapBox(_slamHitboxTransform.position, new Vector3(5, 1, 5), Quaternion.identity, _slamLayerMask);
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

