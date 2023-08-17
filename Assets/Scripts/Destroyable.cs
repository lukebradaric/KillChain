using KillChain.Audio;
using KillChain.Camera;
using KillChain.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace KillChain
{
    public class Destroyable : MonoBehaviour, IDamageable, IDestroyable
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameObject _destroyParticlePrefab;
        [SerializeField] private AudioAsset _destroyAudioAsset;
        [SerializeField] private CameraShakeData _destroyCameraShakeData;

        [Space]
        [Header("Settings")]
        [SerializeField] private int _health = 1;
        [SerializeField] private bool _destroyGameObjectOnDestroy = true;

        [Space]
        [Header("Events")]
        [SerializeField] private UnityEvent _damagedEvent;
        [SerializeField] private UnityEvent _destroyEvent;

        public event Action Destroyed;

        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _health;
        }

        public void Damage(int damage)
        {
            _currentHealth -= damage;

            _damagedEvent?.Invoke();

            if (_currentHealth <= 0)
                Destroy();
        }

        public void Destroy()
        {
            Destroyed?.Invoke();
            _destroyEvent?.Invoke();
            _destroyAudioAsset?.Play();
            _destroyCameraShakeData?.Play();

            if (_destroyParticlePrefab != null) Instantiate(_destroyParticlePrefab, transform.position, Quaternion.identity);

            if (_destroyGameObjectOnDestroy)
                GameObject.Destroy(gameObject);
        }
    }
}

