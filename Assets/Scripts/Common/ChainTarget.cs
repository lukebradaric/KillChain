using KillChain.Core;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace KillChain.Common
{
    public class ChainTarget : MonoBehaviour, IChainTarget
    {
        [Space]
        [Header("Settings")]
        [Tooltip("Can the player dash to this chain target?")]
        [SerializeField] private bool _isDashable;
        [Tooltip("Can the player pull this chain target?")]
        [SerializeField] private bool _isPullable;
        [Tooltip("Will the player get a speed boost when reaching this target?")]
        [SerializeField] private bool _isBoostable;

        [Space]
        [Header("Components")]
        [ShowIf(nameof(_isPullable))]
        [SerializeField] private Rigidbody _rigidbody;

        public bool IsDashable => _isDashable;
        public bool IsPullable => _isPullable;
        public bool IsBoostable => _isBoostable;

        Transform IChainTarget.Transform { get => this.transform; }

        public event Action Destroyed;

        private Transform _pullTargetTransform = null;
        private float _pullSpeed = 0;

        public void StartPull(Transform transform, float speed)
        {
            _pullTargetTransform = transform;
            _pullSpeed = speed;
        }

        public void StopPull()
        {
            _pullTargetTransform = null;
            _pullSpeed = 0;
            _rigidbody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (_pullTargetTransform == null)
            {
                return;
            }

            _rigidbody.velocity = (_pullTargetTransform.position - transform.position).normalized * _pullSpeed;
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}

