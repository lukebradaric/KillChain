using KillChain.Core;
using UnityEngine;

namespace KillChain.Common
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IParryable
    {
        [Space]
        [Header("Components")]
        [Tooltip("Rigidbody of this projectile.")]
        [SerializeField] private Rigidbody _rigidbody;

        [Space]
        [Header("Settings")]
        [Tooltip("Damage this projectile deals.")]
        [SerializeField] private int _damage;
        [Tooltip("Max lifetime of this projectile. (Projectile is destroyed after lifetime)")]
        [SerializeField] private float _maxLifetime;

        private void Start()
        {
            Destroy(gameObject, _maxLifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(_damage);
                Destroy(gameObject);
            }
        }

        public void Parry(float velocityMultiplier = 1f)
        {
            // Parry projectile
            _rigidbody.velocity = UnityEngine.Camera.main.transform.forward * _rigidbody.velocity.magnitude * velocityMultiplier;
        }
    }
}
