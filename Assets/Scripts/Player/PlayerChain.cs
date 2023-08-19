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
        public IChainTarget LookTarget { get; private set; } = null;
        public IChainTarget Target { get; private set; } = null;

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
            if (Target == null && CurrentState.Value != PlayerChainState.Idle)
            {
                CurrentState.Value = PlayerChainState.Idle;
            }

            if (TryGetTarget<IChainTarget>(out IChainTarget chainTarget))
            {
                LookTarget = chainTarget;
            }
            else
            {
                LookTarget = null;
            }

            // If target being pulled is within pull stop range, stop pull
            if (CurrentState.Value == PlayerChainState.Pull && Target.IsPullable)
            {
                if (Vector3.Distance(_player.transform.position, Target.Transform.position) < _player.Data.PullStopDistance)
                {
                    Target.StopPull();
                    Target = null;
                    CurrentState.Value = PlayerChainState.Idle;
                }
            }
        }

        private void FirePressedHandler()
        {
            if (CurrentState.Value != PlayerChainState.Idle)
            {
                return;
            }

            if (LookTarget == null)
            {
                return;
            }

            if (!IsChainTargetInLineOfSight())
            {
                return;
            }

            SetTarget(LookTarget);

            Debug.Log($"Chain Dashing: {Target.Transform.gameObject.name}");
            StartCoroutine(DashDelayCoroutine());
        }

        private void AltFirePressedHandler()
        {
            if (CurrentState.Value != PlayerChainState.Idle)
            {
                return;
            }

            if (LookTarget == null || !LookTarget.IsPullable)
            {
                return;
            }

            if (!IsChainTargetInLineOfSight())
            {
                return;
            }

            SetTarget(LookTarget);

            Debug.Log($"Chain Pulling: {Target.Transform.gameObject.name}");
            StartCoroutine(PullDelayCoroutine());
        }

        // Change state and unsubscribe from events when chain target is destroyed
        private void TargetDestroyedHandler()
        {
            if (Target == null)
            {
                return;
            }

            Target.Destroyed -= TargetDestroyedHandler;
            CurrentState.Value = PlayerChainState.Idle;
            Target = null;
        }

        private void SetTarget(IChainTarget chainTarget)
        {
            Target = chainTarget;
            Target.Destroyed += TargetDestroyedHandler;
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

            Target.StartPull(_player.transform, _player.Data.PullSpeed);
        }

        private bool TryGetTarget<T>(out T ChainTarget)
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

        private bool IsChainTargetInLineOfSight(IChainTarget chainTarget = null)
        {
            if (chainTarget == null)
            {
                if (LookTarget == null)
                {
                    return false;
                }

                chainTarget = LookTarget;
            }

            Physics.Raycast(_player.CameraTransform.position, (chainTarget.Transform.position - _player.CameraTransform.position).normalized, out RaycastHit hit, _player.Data.MaxChainDistance, _breakLayerMask);

            return hit.transform == chainTarget.Transform;
        }

        private float CalculateStateChangeDelay()
        {
            return _player.Data.MaxChainDelayTime / (_player.Data.MaxChainDistance / Vector3.Distance(_player.transform.position, Target.Transform.position));
        }

        public void ForceIdleState()
        {
            CurrentState.Value = PlayerChainState.Idle;
            Target = null;
        }
    }
}

