using KillChain.Core;
using KillChain.Core.Gizmos;
using KillChain.Core.Events;
using System.Collections;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerMelee : PlayerMonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerMeleeEventChannel;
        [SerializeField] private FloatEventChannel _playerMeleeCooldownStartedEventChannel;
        [SerializeField] private VoidEventChannel _playerParryEventChannel;

        [Space]
        [Header("Components")]
        [SerializeField] private GameObject _parryParticlePrefab;

        [Space]
        [Header("Settings")]
        [SerializeField] private LayerMask _meleeLayerMask;

        public bool CanMelee { get; private set; } = true;

        private void OnEnable()
        {
            _player.GameInput.MeleePressed += MeleePressedHandler;
        }

        private void OnDisable()
        {
            _player.GameInput.MeleePressed -= MeleePressedHandler;
        }

        private void MeleePressedHandler()
        {
            if (!CanMelee) return;

            _playerMeleeEventChannel.Invoke();

            StartCoroutine(MeleeCooldownCoroutine());

            Collider[] colliders = Physics.OverlapCapsule(_player.MeleeHitboxTransform.position,
                _player.MeleeHitboxTransform.position + _player.CameraTransform.forward * _player.Data.MeleeLength,
                _player.Data.MeleeRadius,
                _meleeLayerMask);

            if (colliders.Length == 0)
                return;

            foreach (Collider collider in colliders)
            {
                // Do damage
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(_player.Data.MeleeDamage);
                }

                // Do parry
                if (collider.TryGetComponent<IParryable>(out var parryable))
                {
                    parryable.Parry(_player.Data.ParryVelocityMultiplier);
                    _playerParryEventChannel?.Invoke();
                    Instantiate(_parryParticlePrefab, collider.transform.position, Quaternion.identity);
                    _player.TimeManager.TimeStop(_player.Data.ParryTimeStopDuration);
                }
            }
        }

        private IEnumerator MeleeCooldownCoroutine()
        {
            CanMelee = false;
            _playerMeleeCooldownStartedEventChannel.Invoke(_player.Data.MeleeCooldown);
            yield return new WaitForSeconds(_player.Data.MeleeCooldown);
            CanMelee = true;
        }

        private void OnDrawGizmos()
        {
            if (!_player)
            {
                return;
            }

            GizmosExtras.DrawWireCapsule(_player.MeleeHitboxTransform.position,
                _player.MeleeHitboxTransform.position + _player.CameraTransform.forward * _player.Data.MeleeLength,
                _player.Data.MeleeRadius);
        }

    }
}
