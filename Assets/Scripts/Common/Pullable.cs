using KillChain.Core;
using UnityEngine;

namespace KillChain.Common
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pullable : MonoBehaviour, IPullable
    {
        [Space]
        [Header("Components")]
        [SerializeField] private Rigidbody _rigidbody;

        private Transform _pullTransform = null;
        private float _pullSpeed = 0;

        public Transform Transform => transform;

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Pull(Transform transform, float pullSpeed)
        {
            _pullTransform = transform;
            _pullSpeed = pullSpeed;
        }

        public void Stop()
        {
            _pullTransform = null;
            _pullSpeed = 0;
            _rigidbody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (_pullTransform == null)
                return;

            _rigidbody.velocity = (_pullTransform.position - transform.position).normalized * _pullSpeed;
        }
    }
}

