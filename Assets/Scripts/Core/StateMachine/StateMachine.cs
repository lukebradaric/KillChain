using System;
using UnityEngine;

namespace KillChain.Core.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        public State PreviousState { get; protected set; } = null;
        public State CurrentState { get; protected set; }

        public event Action<State> StateChanged;

        public virtual void ChangeState(State newState)
        {
            if (newState.Equals(CurrentState))
            {
                return;
            }

            CurrentState?.Exit();

            PreviousState = CurrentState;

            CurrentState = newState;

            CurrentState.SetStateMachine(this);

            OnStateChanging(newState);

            CurrentState?.Enter();

            StateChanged?.Invoke(CurrentState);
        }

        protected virtual void OnStateChanging(State newState) { }

        protected virtual void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        protected virtual void OnDestroy()
        {
            CurrentState?.Exit();
        }

        protected virtual void OnDrawGizmos()
        {
            CurrentState?.OnDrawGizmos();
        }
    }
}

