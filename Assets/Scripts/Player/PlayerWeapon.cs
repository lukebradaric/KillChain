using ChainKill.Core.Extensions;
using ChainKill.Input;
using System;
using UnityEngine;

namespace ChainKill.Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        [Space]
        [Header("Prefabs")]
        [SerializeField] private GameObject _projectilePrefab;

        [Space]
        [Header("Components")]
        [SerializeField] private GameInput _gameInput;
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LineRenderer _chainLineRenderer;
        [SerializeField] private Transform _chainStartTransform;

        [Space]
        [Header("Settigs")]
        [SerializeField] private LayerMask _enemyLayerMask;

        private PlayerWeaponState _state = PlayerWeaponState.Idle;
        public PlayerWeaponState State
        {
            get => _state;
            private set
            {
                _state = value;
                StateChanged?.Invoke(value);
            }
        }
        public event Action<PlayerWeaponState> StateChanged;

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
            if (State == PlayerWeaponState.Attach)
            {
                State = PlayerWeaponState.Dash;
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _playerSettings.MaxTargetDistance, _enemyLayerMask))
            {
                State = PlayerWeaponState.Attach;
                _attachedEnemy = hit.collider.gameObject;
            }
        }

        private void HandleChainLineRenderer()
        {
            // If idle, disable
            switch (State)
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
            if (State == PlayerWeaponState.Dash)
            {
                _rigidbody.velocity = (_attachedEnemy.transform.position - transform.position).normalized * _playerSettings.DashSpeed;

                if (Vector3.Distance(transform.position, _attachedEnemy.transform.position) < _playerSettings.DashKillDistance)
                {
                    Destroy(_attachedEnemy);
                    _attachedEnemy = null;
                    State = PlayerWeaponState.Idle;
                    _rigidbody.SetVelocityY(0);
                    _rigidbody.AddForce(Vector3.up * _playerSettings.DashKillUpwardForce, ForceMode.Impulse);
                }
            }
        }
    }
}
