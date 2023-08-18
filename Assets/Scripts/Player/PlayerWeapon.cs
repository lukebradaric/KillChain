using KillChain.Core;
using KillChain.Core.Events;
using KillChain.Core.Extensions;
using KillChain.Core.Generics;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerWeapon : PlayerMonoBehaviour
    {
        [Space]
        [Header("EventChannels")]
        [SerializeField] private VoidEventChannel _playerChainBrokeEventChannel;

        [Space]
        [Header("Settings")]
        [SerializeField] private LayerMask _chainableLayerMask;
        [SerializeField] private LayerMask _chainBreakLayerMask;

        public Observable<PlayerWeaponState> State { get; private set; } = new Observable<PlayerWeaponState>(PlayerWeaponState.Idle);

        public IChainable CurrentChainable => _currentChainable;

        private IChainable _currentChainable = null;
        private IPullable _currentPullable = null;
        private IDestroyable _currentDestroyable = null;

        private void OnEnable()
        {
            _player.GameInput.FirePressed += FirePressedHandler;
            _player.GameInput.AltFirePressed += AltFirePressedHandler;
        }

        private void OnDisable()
        {
            _player.GameInput.FirePressed -= FirePressedHandler;
            _player.GameInput.AltFirePressed -= AltFirePressedHandler;
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
            // Raycast to check if player hit a chainable object
            RaycastHit hit;
            Physics.Raycast(_player.CameraTransform.position, _player.CameraTransform.forward, out hit, _player.Data.MaxTargetDistance, _chainableLayerMask);

            if (hit.collider != null && hit.collider.TryGetComponent<IChainable>(out var chainable))
            {
                // TODO (001) : lol this is ass, fix after testing
                if (Physics.Raycast(_player.CameraTransform.position, (chainable.Transform.position - _player.CameraTransform.position).normalized, out var hitCenter, _player.Data.MaxTargetDistance, _chainBreakLayerMask))
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
                pullable.Pull(transform, _player.Data.PullSpeed);

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
                    _player.ChainLineRenderer.enabled = false;
                    break;
                case PlayerWeaponState.Attach:
                case PlayerWeaponState.Dash:
                case PlayerWeaponState.Pull:
                    _player.ChainLineRenderer.enabled = true;
                    _player.ChainLineRenderer.SetPosition(0, _player.ChainStartTransform.position);
                    _player.ChainLineRenderer.SetPosition(1, _currentChainable.Transform.position);
                    break;
            }
        }

        private void HandleChainBreakCheck()
        {
            // Chain Break Check
            if (State.Value == PlayerWeaponState.Attach || State.Value == PlayerWeaponState.Dash)
            {
                if (Physics.Raycast(_player.CameraTransform.position, (_currentChainable.Transform.position - _player.CameraTransform.position).normalized, out var hit, _player.Data.MaxTargetDistance, _chainBreakLayerMask))
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
                _player.Rigidbody.velocity = (_currentChainable.Transform.position - transform.position).normalized * _player.Data.DashSpeed;

                if (Vector3.Distance(transform.position, _currentChainable.Transform.position) < _player.Data.DashStopDistance)
                {
                    // If chainable is also damageable, damage and add upwards force
                    if (_currentChainable.Transform.TryGetComponent<IDamageable>(out var damageable))
                    {
                        damageable.Damage(_player.Data.DashDamage);
                        _player.Rigidbody.SetVelocityY(0);
                        _player.Rigidbody.AddForce(Vector3.up * _player.Data.DashDamageUpwardForce, ForceMode.Impulse);
                    }
                    else
                    {
                        // Bounce player off of target they were dashing to
                        _player.Rigidbody.velocity = -_player.Rigidbody.velocity.normalized * _player.Data.DashReboundSpeed;
                        _player.Rigidbody.SetVelocityY(_player.Data.DashReboundUpwardForce);
                    }

                    _currentChainable = null;
                    State.Value = PlayerWeaponState.Idle;
                }
            }

            // Pull State
            if (State.Value == PlayerWeaponState.Pull)
            {
                if (Vector3.Distance(transform.position, _currentPullable.Transform.position) < _player.Data.PullStopDistance)
                {
                    _currentPullable.Stop();
                    _currentPullable = null;
                    State.Value = PlayerWeaponState.Idle;
                }
            }
        }
    }
}
