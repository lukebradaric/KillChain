using KillChain.Audio;
using KillChain.Camera;
using KillChain.Core.Extensions;
using KillChain.Core.Generics;
using KillChain.Input;
using System;
using System.Collections;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _chainStartTransform;
        [SerializeField] private LineRenderer _chainLineRenderer;

        [Space]
        [Header("Settings")]
        [SerializeField] private LayerMask _enemyLayerMask;

        [Space]
        [Header("Debug")]
        // TODO : Remove these debug variables once enemies are actually implemented
        public AudioAsset DEBUG_enemyDeathAudioAsset;
        public GameObject DEBUG_enemyDeathParticlePrefab;
        public CameraShakeData DEBUG_enemyDeathCameraShakeData;

        public bool ChainOnCooldown { get; private set; }

        public static Observable<PlayerWeaponState> State { get; private set; } = new Observable<PlayerWeaponState>(PlayerWeaponState.Idle);

        private GameObject _attachedEnemy = null;

        private void OnEnable()
        {
            _gameInput.FirePressed += FirePressedHandler;
        }

        private void OnDisable()
        {
            _gameInput.FirePressed -= FirePressedHandler;
        }

        private void Update()
        {
            HandleChainLineRenderer();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void FirePressedHandler()
        {
            if (State.Value == PlayerWeaponState.Attach)
            {
                State.Value = PlayerWeaponState.Dash;
                return;
            }

            if (ChainOnCooldown)
            {
                return;
            }

            StartCoroutine(ChainCooldownCoroutine());

            RaycastHit hit;
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, _playerData.MaxTargetDistance, _enemyLayerMask))
            {
                State.Value = PlayerWeaponState.Attach;
                _attachedEnemy = hit.collider.gameObject;
            }
        }

        private IEnumerator ChainCooldownCoroutine()
        {
            ChainOnCooldown = true;

            if (State.Value != PlayerWeaponState.Attach)
                State.Value = PlayerWeaponState.Miss;

            yield return new WaitForSeconds(_playerData.ChainCooldown);

            if (State.Value == PlayerWeaponState.Miss)
                State.Value = PlayerWeaponState.Idle;

            ChainOnCooldown = false;
        }

        private void HandleChainLineRenderer()
        {
            // If idle, disable
            switch (State.Value)
            {
                case PlayerWeaponState.Idle:
                    _chainLineRenderer.enabled = false;
                    break;
                case PlayerWeaponState.Attach:
                case PlayerWeaponState.Dash:
                    _chainLineRenderer.enabled = true;
                    _chainLineRenderer.SetPosition(0, _chainStartTransform.position);
                    _chainLineRenderer.SetPosition(1, _attachedEnemy.transform.position);
                    break;
                case PlayerWeaponState.Pull:
                    break;
            }
        }

        private void HandleMovement()
        {
            if (State.Value == PlayerWeaponState.Dash)
            {
                _rigidbody.velocity = (_attachedEnemy.transform.position - transform.position).normalized * _playerData.DashSpeed;

                if (Vector3.Distance(transform.position, _attachedEnemy.transform.position) < _playerData.DashKillDistance)
                {
                    // TODO : Replace this with an actual enemy implementation
                    Instantiate(DEBUG_enemyDeathParticlePrefab, _attachedEnemy.transform.position, Quaternion.identity);
                    DEBUG_enemyDeathCameraShakeData?.Play();
                    Destroy(_attachedEnemy);
                    DEBUG_enemyDeathAudioAsset?.Play();
                    _attachedEnemy = null;
                    State.Value = PlayerWeaponState.Idle;
                    _rigidbody.SetVelocityY(0);
                    _rigidbody.AddForce(Vector3.up * _playerData.DashKillUpwardForce, ForceMode.Impulse);
                }
            }
        }
    }
}
