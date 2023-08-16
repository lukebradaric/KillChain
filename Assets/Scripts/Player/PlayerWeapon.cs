using KillChain.Core.Extensions;
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
        [SerializeField] private LineRenderer _chainLineRenderer;
        [SerializeField] private Transform _chainStartTransform;

        [Space]
        [Header("Settigs")]
        [SerializeField] private LayerMask _enemyLayerMask;

        public bool ChainOnCooldown { get; private set; }

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

            if (ChainOnCooldown)
            {
                return;
            }

            StartCoroutine(ChainCooldownCoroutine());

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _playerData.MaxTargetDistance, _enemyLayerMask))
            {
                State = PlayerWeaponState.Attach;
                _attachedEnemy = hit.collider.gameObject;
            }
        }

        private IEnumerator ChainCooldownCoroutine()
        {
            ChainOnCooldown = true;

            if (State != PlayerWeaponState.Attach)
                State = PlayerWeaponState.Miss;

            yield return new WaitForSeconds(_playerData.ChainCooldown);

            if (State == PlayerWeaponState.Miss)
                State = PlayerWeaponState.Idle;

            ChainOnCooldown = false;
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
                _rigidbody.velocity = (_attachedEnemy.transform.position - transform.position).normalized * _playerData.DashSpeed;

                if (Vector3.Distance(transform.position, _attachedEnemy.transform.position) < _playerData.DashKillDistance)
                {
                    // TODO : Replace this with an actual enemy implementation
                    Destroy(_attachedEnemy);
                    _attachedEnemy = null;
                    State = PlayerWeaponState.Idle;
                    _rigidbody.SetVelocityY(0);
                    _rigidbody.AddForce(Vector3.up * _playerData.DashKillUpwardForce, ForceMode.Impulse);
                }
            }
        }
    }
}
