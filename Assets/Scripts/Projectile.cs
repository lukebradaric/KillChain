using KillChain.Audio;
using KillChain.Camera;
using KillChain.Core;
using KillChain.Core.Managers;
using UnityEngine;

namespace KillChain
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IDamageable
    {
        [Space]
        [Header("Components")]
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CameraShakeData _parryCameraShakeData;
        [SerializeField] private GameObject _parryParticlePrefab;
        [SerializeField] private AudioAsset _parryAudioAsset;

        [Space]
        [Header("Settings")]
        [SerializeField] private int _damage;
        [SerializeField] private float _parrySpeedMultiplier = 2f;
        [SerializeField] private float _parryTimeStopDuration;

        public void Damage(int damage)
        {
            Instantiate(_parryParticlePrefab, transform.position, Quaternion.identity);
            _parryCameraShakeData?.Play();
            _parryAudioAsset?.Play();
            _rigidbody.velocity = UnityEngine.Camera.main.transform.forward * _rigidbody.velocity.magnitude * _parrySpeedMultiplier;
            _timeManager.StopTime(_parryTimeStopDuration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(_damage);
                Destroy(gameObject);
            }
        }
    }
}

