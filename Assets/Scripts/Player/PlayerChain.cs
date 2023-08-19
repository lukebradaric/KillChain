using KillChain.Core;
using KillChain.Core.Generics;
using KillChain.Player.States;
using System.Collections;
using UnityEngine;

namespace KillChain.Player
{
    public class PlayerChain : PlayerMonoBehaviour
    {
        [Space]
        [Header("Settings")]
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] private LayerMask _breakLayerMask;

        public Observable<PlayerChainState> CurrentState { get; private set; } = new Observable<PlayerChainState>(PlayerChainState.Idle);

        // TODO : Rework chainable, to be single script instead of multiple interfaces
        public IChainable Target;
        public IChainable LookTarget;
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
                LookTarget = chainable;
            }
            else
            {
                LookTarget = null;
            }

            if (CurrentState.Value == PlayerChainState.Pull)
            {
                if (Vector3.Distance(_player.transform.position, _currentPullable.Transform.position) < _player.Data.PullStopDistance)
                {
                    _currentPullable.Stop();
                    _currentPullable = null;
                    Target = null;
                    CurrentState.Value = PlayerChainState.Idle;
                }
            }
        }

        private void FirePressedHandler()
        {
            if (LookTarget == null)
            {
                return;
            }

            if (!ChainTargetInLineOfSight())
            {
                return;
            }

            Target = LookTarget;

            Debug.Log($"Chain Dashing: {Target.Transform.gameObject.name}");
            StartCoroutine(DashDelayCoroutine());
        }

        private void AltFirePressedHandler()
        {
            if (LookTarget == null || !LookTarget.Transform.TryGetComponent<IPullable>(out IPullable pullable))
            {
                return;
            }

            if (!ChainTargetInLineOfSight())
            {
                return;
            }

            Target = LookTarget;
            _currentPullable = pullable;

            Debug.Log($"Chain Pulling: {Target.Transform.gameObject.name}");
            StartCoroutine(PullDelayCoroutine());
        }

        private IEnumerator DashDelayCoroutine()
        {
            CurrentState.Value = PlayerChainState.Throw;
            yield return new WaitForSeconds(CalculateStateChangeDelay());
            CurrentState.Value = PlayerChainState.Dash;

            // TODO: Invoke event instead of directly changing player state
            _player.StateMachine.ChangeState(_player.StateMachine.DashState);
        }

        private IEnumerator PullDelayCoroutine()
        {
            CurrentState.Value = PlayerChainState.Throw;
            yield return new WaitForSeconds(CalculateStateChangeDelay());
            CurrentState.Value = PlayerChainState.Pull;

            _currentPullable.Pull(_player.transform, _player.Data.PullSpeed);
        }

        public bool TryGetTarget<T>(out T target)
        {
            target = default(T);
            Physics.Raycast(_player.CameraTransform.position, _player.CameraTransform.forward, out RaycastHit hit, _player.Data.MaxChainDistance, _targetLayerMask);

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
                if (LookTarget == null)
                {
                    return false;
                }

                chainable = LookTarget;
            }

            Physics.Raycast(_player.CameraTransform.position, (chainable.Transform.position - _player.CameraTransform.position).normalized, out RaycastHit hit, _player.Data.MaxChainDistance, _breakLayerMask);

            return hit.transform == chainable.Transform;
        }

        public float CalculateStateChangeDelay()
        {
            return _player.Data.MaxChainDelayTime / (_player.Data.MaxChainDistance / Vector3.Distance(_player.transform.position, Target.Transform.position));
        }
    }
}

