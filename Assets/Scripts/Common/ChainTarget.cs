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
        [SerializeField] private bool _isDashable;
        [SerializeField] private bool _isPullable;

        [Space]
        [Header("Components")]
        [ShowIf(nameof(_isPullable))]
        [SerializeField] private Rigidbody _rigidbody;

        bool IChainTarget.IsDashable { get => _isDashable; }
        bool IChainTarget.IsPullable { get => _isPullable; }

        Transform IChainTarget.Transform { get => this.transform; }

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
        }

        private void FixedUpdate()
        {
            if (_pullTargetTransform == null)
            {
                return;
            }

            _rigidbody.velocity = (_pullTargetTransform.position - transform.position).normalized * _pullSpeed;
        }
    }
}

