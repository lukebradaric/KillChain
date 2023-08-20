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

        // Was the fire key released
        private bool _fireReleased = false;
        private bool _altFireReleased = false;

        private Coroutine _dashDelayCoroutine;
        private Coroutine _pullDelayCoroutine;

        private void OnEnable()
        {
            _player.GameInput.FirePressed += FirePressedHandler;
            _player.GameInput.FireReleased += FireReleasedHandler;
            _player.GameInput.AltFirePressed += AltFirePressedHandler;
            _player.GameInput.AltFireReleased += AltFireReleasedHandler;
        }

        private void OnDisable()
        {
            _player.GameInput.FirePressed -= FirePressedHandler;
            _player.GameInput.FireReleased += FireReleasedHandler;
            _player.GameInput.AltFirePressed -= AltFirePressedHandler;
            _player.GameInput.AltFireReleased += AltFireReleasedHandler;
        }

        private void FixedUpdate()
        {
            UpdateLookTarget();

            UpdatePullState();

            UpdateLineOfSightCheck();
        }

        private void FirePressedHandler()
        {
            _fireReleased = false;

            if (!CanEnterNewChainState())
            {
                return;
            }

            if (!LookTarget.IsDashable)
            {
                return;
            }

            SetTarget(LookTarget);

            _dashDelayCoroutine = StartCoroutine(DashDelayCoroutine());
        }

        private void FireReleasedHandler()
        {
            _fireReleased = true;
        }

        private void AltFirePressedHandler()
        {
            _altFireReleased = false;

            if (!CanEnterNewChainState())
            {
                return;
            }

            if (!LookTarget.IsPullable)
            {
                return;
            }

            SetTarget(LookTarget);

            _pullDelayCoroutine = StartCoroutine(PullDelayCoroutine());
        }

        private void AltFireReleasedHandler()
        {
            _altFireReleased = true;
            StopPullState();
        }

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

            // If the fire key was released before reaching dash state, cancel state change and go to idle
            if (_fireReleased)
            {
                ForceIdleState();
                _fireReleased = false;
                yield break;
            }

            // TODO: Invoke event instead of directly changing player state
            _player.StateMachine.ChangeState(_player.StateMachine.DashState);
        }

        private IEnumerator PullDelayCoroutine()
        {
            CurrentState.Value = PlayerChainState.Throw;
            yield return new WaitForSeconds(CalculateStateChangeDelay());
            CurrentState.Value = PlayerChainState.Pull;

            // If alt fire key was released before reaching pull state, cancel state change and go to idle
            if (_altFireReleased)
            {
                ForceIdleState();
                _altFireReleased = false;
                yield break;
            }

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

        private bool IsTargetInLineOfSight(IChainTarget chainTarget)
        {
            if (chainTarget == null)
            {
                Debug.LogWarning("Target provided was null.");
                return false;
            }

            Physics.Raycast(_player.CameraTransform.position, (chainTarget.Transform.position - _player.CameraTransform.position).normalized, out RaycastHit hit, _player.Data.MaxChainDistance, _breakLayerMask);

            return hit.transform == chainTarget.Transform;
        }

        private float CalculateStateChangeDelay()
        {
            return _player.Data.MaxChainDelayTime / (_player.Data.MaxChainDistance / Vector3.Distance(_player.transform.position, Target.Transform.position));
        }

        private bool CanEnterNewChainState()
        {
            if (CurrentState.Value != PlayerChainState.Idle)
            {
                return false;
            }

            if (LookTarget == null)
            {
                return false;
            }

            if (!IsTargetInLineOfSight(LookTarget))
            {
                return false;
            }

            return true;
        }

        private void UpdateLineOfSightCheck()
        {
            if (Target == null)
            {
                return;
            }

            if (IsTargetInLineOfSight(Target))
            {
                return;
            }

            // Chain Break Event

            // If line of sight broken while throwing
            if (CurrentState.Value == PlayerChainState.Throw)
            {
                if (_dashDelayCoroutine != null)
                {
                    StopCoroutine(_dashDelayCoroutine);
                }

                if (_pullDelayCoroutine != null)
                {
                    StopCoroutine(_pullDelayCoroutine);
                }

                ForceIdleState();
            }

            // If line of sight broken while pulling
            if (CurrentState.Value == PlayerChainState.Pull)
            {
                StopPullState();
                return;
            }

            // If line of sight broken while dashing
            if (CurrentState.Value == PlayerChainState.Dash)
            {
                if (_player.GroundCheck.IsFound())
                {
                    _player.StateMachine.ChangeState(_player.StateMachine.AirState);
                }
                else
                {
                    _player.StateMachine.ChangeState(_player.StateMachine.MoveState);
                }

                ForceIdleState();
            }
        }

        private void UpdateLookTarget()
        {
            if (TryGetTarget<IChainTarget>(out IChainTarget chainTarget))
            {
                LookTarget = chainTarget;
            }
            else
            {
                LookTarget = null;
            }
        }

        private void UpdatePullState()
        {
            // If target being pulled is within pull stop range, stop pull
            if (CurrentState.Value == PlayerChainState.Pull && Target.IsPullable)
            {
                if (Vector3.Distance(_player.transform.position, Target.Transform.position) < _player.Data.PullStopDistance)
                {
                    StopPullState();
                }
            }
        }

        private void StopPullState()
        {
            if (CurrentState.Value != PlayerChainState.Pull)
            {
                return;
            }

            _altFireReleased = false;

            Target.StopPull();
            ForceIdleState();
        }

        public void ForceIdleState()
        {
            CurrentState.Value = PlayerChainState.Idle;
            Target = null;
        }
    }
}

