using KillChain.Core;
using KillChain.Core.Common;
using KillChain.Core.Events;
using KillChain.Core.Managers;
using KillChain.Input;
using System.Collections;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerMelee : MonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerMeleeEventChannel;
        [SerializeField] private FloatEventChannel _playerMeleeCooldownStartedEventChannel;

        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private Transform _cameraTransform;

        [Space]
        [Header("Settings")]
        [SerializeField] private Vector3 _hitCapsuleStartPosition;
        [SerializeField] private LayerMask _meleeLayerMask;

        public bool CanMelee { get; private set; } = true;

        private void OnEnable()
        {
            _gameInput.MeleePressed += MeleePressedHandler;
        }

        private void OnDisable()
        {
            _gameInput.MeleePressed -= MeleePressedHandler;
        }

        private void MeleePressedHandler()
        {
            if (!CanMelee) return;

            _playerMeleeEventChannel.Invoke();

            StartCoroutine(MeleeCooldownCoroutine());

            Collider[] colliders = Physics.OverlapCapsule(transform.position + _hitCapsuleStartPosition,
                transform.position + _hitCapsuleStartPosition + _cameraTransform.forward * _playerData.MeleeLength,
                _playerData.MeleeRadius,
                _meleeLayerMask);

            if (colliders.Length == 0)
                return;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                    damageable.Damage(_playerData.MeleeDamage);
            }
        }

        private IEnumerator MeleeCooldownCoroutine()
        {
            CanMelee = false;
            _playerMeleeCooldownStartedEventChannel.Invoke(_playerData.MeleeCooldown);
            yield return new WaitForSeconds(_playerData.MeleeCooldown);
            CanMelee = true;
        }

        private void OnDrawGizmos()
        {
            GizmosExtras.DrawWireCapsule(transform.position + _hitCapsuleStartPosition,
                transform.position + _hitCapsuleStartPosition + _cameraTransform.forward * _playerData.MeleeLength,
                _playerData.MeleeRadius);
        }

    }
}
