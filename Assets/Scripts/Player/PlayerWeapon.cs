using KillChain.Core;
using KillChain.Core.Events;
using KillChain.Core.Extensions;
using KillChain.Core.Generics;
using KillChain.Input;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerChainBrokeEventChannel;

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
        [SerializeField] private LayerMask _chainableLayerMask;
        [SerializeField] private LayerMask _chainBreakLayerMask;

        public Observable<PlayerWeaponState> State { get; private set; } = new Observable<PlayerWeaponState>(PlayerWeaponState.Idle);

        private IChainable _currentChainable = null;
        private IPullable _currentPullable = null;
        private IDestroyable _currentDestroyable = null;

        private void OnEnable()
        {
            _gameInput.FirePressed += FirePressedHandler;
            _gameInput.AltFirePressed += AltFirePressedHandler;
        }

        private void OnDisable()
        {
            _gameInput.FirePressed -= FirePressedHandler;
            _gameInput.AltFirePressed -= AltFirePressedHandler;
        }

        private void Update()
        {
            HandleChainLineRenderer();
        }

        private void FixedUpdate()
        {
            HandleStates();
            HandleChainBreakCheck();
        }

        private void FirePressedHandler()
        {
            // If player already attached, enter dash state
            if (State.Value == PlayerWeaponState.Attach)
            {
                State.Value = PlayerWeaponState.Dash;
                return;
            }

            // Raycast to check if player hit a chainable object
            RaycastHit hit;
            Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, _playerData.MaxTargetDistance, _chainableLayerMask);
            if (hit.collider != null && hit.collider.TryGetComponent<IChainable>(out var chainable))
            {
                // TODO (001) : lol this is ass, fix after testing
                if (Physics.Raycast(_cameraTransform.position, (chainable.Transform.position - _cameraTransform.position).normalized, out var hitCenter, _playerData.MaxTargetDistance, _chainBreakLayerMask))
                {
                    if (hitCenter.transform != null && hitCenter.transform == chainable.Transform)
                    {
                        State.Value = PlayerWeaponState.Attach;
                        _currentChainable = chainable;
                    }
                }
            }
        }

        private void AltFirePressedHandler()
        {
            if (State.Value != PlayerWeaponState.Attach)
            {
                return;
            }

            if (_currentChainable.Transform.TryGetComponent<IPullable>(out var pullable))
            {
                _currentPullable = pullable;
                State.Value = PlayerWeaponState.Pull;
                pullable.Pull(transform, _playerData.PullSpeed);

                if (pullable.Transform.TryGetComponent<IDestroyable>(out var destroyable))
                {
                    _currentDestroyable = destroyable;
                    destroyable.Destroyed += PullableDestroyedHandler;
                }
            }
        }

        private void PullableDestroyedHandler()
        {
            _currentDestroyable.Destroyed -= PullableDestroyedHandler;
            _currentPullable = null;
            State.Value = PlayerWeaponState.Idle;
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
                case PlayerWeaponState.Pull:
                    _chainLineRenderer.enabled = true;
                    _chainLineRenderer.SetPosition(0, _chainStartTransform.position);
                    _chainLineRenderer.SetPosition(1, _currentChainable.Transform.position);
                    break;
            }
        }

        private void HandleChainBreakCheck()
        {
            // Chain Break Check
            if (State.Value == PlayerWeaponState.Attach || State.Value == PlayerWeaponState.Dash)
            {
                if (Physics.Raycast(_cameraTransform.position, (_currentChainable.Transform.position - _cameraTransform.position).normalized, out var hit, _playerData.MaxTargetDistance, _chainBreakLayerMask))
                {
                    if (hit.transform != _currentChainable.Transform)
                    {
                        State.Value = PlayerWeaponState.Idle;
                        _playerChainBrokeEventChannel.Invoke();
                        _currentChainable = null;
                    }
                }
            }
        }

        private void HandleStates()
        {
            // Dash State
            if (State.Value == PlayerWeaponState.Dash)
            {
                _rigidbody.velocity = (_currentChainable.Transform.position - transform.position).normalized * _playerData.DashSpeed;

                if (Vector3.Distance(transform.position, _currentChainable.Transform.position) < _playerData.DashKillDistance)
                {
                    // If chainable is also damageable, damage and add upwards force
                    if (_currentChainable.Transform.TryGetComponent<IDamageable>(out var damageable))
                    {
                        damageable.Damage(_playerData.DashDamage);
                        _rigidbody.SetVelocityY(0);
                        _rigidbody.AddForce(Vector3.up * _playerData.DashKillUpwardForce, ForceMode.Impulse);
                    }

                    _currentChainable = null;

                    State.Value = PlayerWeaponState.Idle;
                }
            }

            // Pull State
            if (State.Value == PlayerWeaponState.Pull)
            {
                if (Vector3.Distance(transform.position, _currentPullable.Transform.position) < _playerData.PullStopDistance)
                {
                    _currentPullable.Stop();
                    _currentPullable = null;
                    State.Value = PlayerWeaponState.Idle;
                }
            }
        }
    }
}
