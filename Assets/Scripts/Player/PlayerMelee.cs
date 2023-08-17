using KillChain.Core;
using KillChain.Core.Common;
using KillChain.Core.Managers;
using KillChain.Input;
using System;
using System.Collections;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerMelee : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private Transform _cameraTransform;

        [Space]
        [Header("Settings")]
        [SerializeField] private int _meleeDamage = 1;
        [SerializeField] private float _meleeCooldown = 0.75f;
        [SerializeField] private LayerMask _meleeLayerMask;
        [SerializeField] private Vector3 _hitCapsuleStartPosition;
        [SerializeField] private float _hitCapsuleLength;
        [SerializeField] private float _hitCapsuleRadius;

        public static event Action MeleeStarted;
        public static event Action MeleeCompleted;
        public static event Action<float> MeleeCooldownStarted;

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

            MeleeStarted?.Invoke();

            StartCoroutine(MeleeCooldownCoroutine());

            Collider[] colliders = Physics.OverlapCapsule(transform.position + _hitCapsuleStartPosition,
                transform.position + _hitCapsuleStartPosition + _cameraTransform.forward * _hitCapsuleLength,
                _hitCapsuleRadius,
                _meleeLayerMask);

            if (colliders.Length == 0)
                return;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                    damageable.Damage(_meleeDamage);
            }

            MeleeCompleted?.Invoke();
        }

        private IEnumerator MeleeCooldownCoroutine()
        {
            CanMelee = false;
            MeleeCooldownStarted?.Invoke(_meleeCooldown);
            yield return new WaitForSeconds(_meleeCooldown);
            CanMelee = true;
        }

        private void OnDrawGizmos()
        {
            GizmosExtras.DrawWireCapsule(transform.position + _hitCapsuleStartPosition,
                transform.position + _hitCapsuleStartPosition + _cameraTransform.forward * _hitCapsuleLength,
                _hitCapsuleRadius);
        }

    }
}
