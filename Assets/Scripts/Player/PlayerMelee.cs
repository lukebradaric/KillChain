using KillChain.Core;
using KillChain.Core.Common;
using KillChain.Core.Managers;
using KillChain.Input;
using System;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerMelee : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private TimeManager _timeManager;

        [Space]
        [Header("Settings")]
        [SerializeField] private float _timeStopDuration = 0.2f;
        [SerializeField] private Vector3 _hitCapsuleStartPosition;
        [SerializeField] private float _hitCapsuleLength;
        [SerializeField] private float _hitCapsuleRadius;
        [SerializeField] private LayerMask _meleeLayerMask;

        public static event Action Meleed;

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
            Meleed?.Invoke();

            Collider[] colliders = Physics.OverlapCapsule(transform.position + _hitCapsuleStartPosition,
                transform.position + _hitCapsuleStartPosition + Camera.main.transform.forward * _hitCapsuleLength,
                _hitCapsuleRadius,
                _meleeLayerMask);

            if (colliders.Length == 0)
                return;

            foreach (Collider collider in colliders)
            {
                // TODO : Consider using an alternative interface? (IDamageable?)
                if (collider.TryGetComponent<IMeleeable>(out var meleeable))
                    meleeable.Melee();
            }

            _timeManager.StopTime(_timeStopDuration);
        }

        private void OnDrawGizmos()
        {
            GizmosExtras.DrawWireCapsule(transform.position + _hitCapsuleStartPosition,
                transform.position + _hitCapsuleStartPosition + Camera.main.transform.forward * _hitCapsuleLength,
                _hitCapsuleRadius);
        }

    }
}
