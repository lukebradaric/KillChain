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
        public IChainTarget LookChainTarget = null;
        public IChainTarget ChainTarget = null;

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
            if(ChainTarget == null && CurrentState.Value != PlayerChainState.Idle)
            {
                CurrentState.Value = PlayerChainState.Idle;
            }

            if (TryGetTarget<IChainTarget>(out IChainTarget chainTarget))
            {
                LookChainTarget = chainTarget;
            }
            else
            {
                LookChainTarget = null;
            }

            if (CurrentState.Value == PlayerChainState.Pull && ChainTarget.IsPullable)
            {
                if (Vector3.Distance(_player.transform.position, ChainTarget.Transform.position) < _player.Data.PullStopDistance)
                {
                    ChainTarget.StopPull();
                    ChainTarget = null;
                    CurrentState.Value = PlayerChainState.Idle;
                }
            }
        }

        private void FirePressedHandler()
        {
            if (LookChainTarget == null)
            {
                return;
            }

            if (!ChainTargetInLineOfSight())
            {
                return;
            }

            ChainTarget = LookChainTarget;

            Debug.Log($"Chain Dashing: {ChainTarget.Transform.gameObject.name}");
            StartCoroutine(DashDelayCoroutine());
        }

        private void AltFirePressedHandler()
        {
            if (LookChainTarget == null || !LookChainTarget.IsPullable)
            {
                return;
            }

            if (!ChainTargetInLineOfSight())
            {
                return;
            }

            ChainTarget = LookChainTarget;

            Debug.Log($"Chain Pulling: {ChainTarget.Transform.gameObject.name}");
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

            ChainTarget.StartPull(_player.transform, _player.Data.PullSpeed);
        }

        public bool TryGetTarget<T>(out T ChainTarget)
        {
            ChainTarget = default(T);
            Physics.Raycast(_player.CameraTransform.position, _player.CameraTransform.forward, out RaycastHit hit, _player.Data.MaxChainDistance, _targetLayerMask);

            if (hit.collider == null || !hit.collider.TryGetComponent<T>(out T tar))
            {
                return false;
            }

            ChainTarget = tar;
            return true;
        }

        public bool ChainTargetInLineOfSight(IChainTarget chainTarget = null)
        {
            if (chainTarget == null)
            {
                if (LookChainTarget == null)
                {
                    return false;
                }

                chainTarget = LookChainTarget;
            }

            Physics.Raycast(_player.CameraTransform.position, (chainTarget.Transform.position - _player.CameraTransform.position).normalized, out RaycastHit hit, _player.Data.MaxChainDistance, _breakLayerMask);

            return hit.transform == chainTarget.Transform;
        }

        public float CalculateStateChangeDelay()
        {
            return _player.Data.MaxChainDelayTime / (_player.Data.MaxChainDistance / Vector3.Distance(_player.transform.position, ChainTarget.Transform.position));
        }
    }
}

