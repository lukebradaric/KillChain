namespace KillChain.Core.StateMachine
{
    public abstract class State
    {
        protected StateMachine _stateMachine;

        public void SetStateMachine(StateMachine stateMachine) => _stateMachine = stateMachine;

        public abstract void Enter();
        public abstract void FixedUpdate();
        public abstract void Exit();

        public virtual void OnDrawGizmos() { }
    }
}

