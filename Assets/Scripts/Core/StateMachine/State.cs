namespace KillChain.Core.StateMachine
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void FixedUpdate();
        public abstract void Exit();

        public virtual void OnDrawGizmos() { }
    }
}

