using KillChain.Core;
using KillChain.Player.States;
using System.Collections;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerChain : PlayerMonoBehaviour
    {
        [Space]
        [Header("Settings")]
        [SerializeField] private LayerMask _chainTargetLayerMask;
        [SerializeField] private LayerMask _chainBreakLayerMask;
        [SerializeField] private float _maxDelayTime;

        public static PlayerChainState CurrentState { get; private set; } = PlayerChainState.Idle;

        public IChainable _currentChainable;
        public IPullable _currentPullable;

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

        private void FixedUpdate()
        {
            if (TryGetTarget<IChainable>(out IChainable chainable))
            {
                _currentChainable = chainable;
            }
            else
            {
                _currentChainable = null;
            }
        }

        private void FirePressedHandler()
        {
            if (_currentChainable == null)
            {
                return;
            }

            if (!ChainTargetInLineOfSight())
            {
                return;
            }

            Debug.Log($"Chain Dashing: {_currentChainable.Transform.gameObject.name}");
            StartCoroutine(DashDelayCoroutine());
        }

        private void AltFirePressedHandler()
        {
            if (_currentChainable == null || !_currentChainable.Transform.TryGetComponent<IPullable>(out IPullable pullable))
            {
                return;
            }

            if (!ChainTargetInLineOfSight())
            {
                return;
            }

            Debug.Log($"Chain Pulling: {_currentChainable.Transform.gameObject.name}");
            StartCoroutine(PullDelayCoroutine());
        }

        private IEnumerator DashDelayCoroutine()
        {
            _player.Weapon.State.Value = PlayerWeaponState.Attach;
            yield return new WaitForSeconds(CalculateStateChangeDelay());
            _player.Weapon.State.Value = PlayerWeaponState.Dash;
            _player.StateMachine.ChangeState(_player.StateMachine.DashState);
        }

        private IEnumerator PullDelayCoroutine()
        {
            _player.Weapon.State.Value = PlayerWeaponState.Attach;
            yield return new WaitForSeconds(CalculateStateChangeDelay());
            _player.Weapon.State.Value = PlayerWeaponState.Pull;
            _currentPullable.Pull(_player.transform, _player.Data.PullSpeed);
        }

        public bool TryGetTarget<T>(out T target)
        {
            target = default(T);
            Physics.Raycast(_player.CameraTransform.position, _player.CameraTransform.forward, out RaycastHit hit, _player.Data.MaxTargetDistance, _chainTargetLayerMask);

            if (hit.collider == null || !hit.collider.TryGetComponent<T>(out T tar))
            {
                return false;
            }

            target = tar;
            return true;
        }

        public bool ChainTargetInLineOfSight(IChainable chainable = null)
        {
            if (chainable == null)
            {
                if (_currentChainable == null)
                {
                    return false;
                }

                chainable = _currentChainable;
            }

            Physics.Raycast(_player.CameraTransform.position, (chainable.Transform.position - _player.CameraTransform.position).normalized, out RaycastHit hit, _player.Data.MaxTargetDistance, _chainBreakLayerMask);

            return hit.transform == chainable.Transform;
        }

        public float CalculateStateChangeDelay()
        {
            return _maxDelayTime / (_player.Data.MaxTargetDistance / Vector3.Distance(_player.transform.position, _currentChainable.Transform.position));
        }
    }
}

