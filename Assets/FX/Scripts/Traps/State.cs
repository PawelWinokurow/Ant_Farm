namespace TrapNamespace
{
    public enum STATE
    {
        IDLE, ATTACK, DEAD
    }

    public abstract class State
    {
        protected Trap trap;

        public STATE type { get; set; }
        public abstract void Tick();

        public abstract void CancelJob();
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }

        public State(Trap trap)
        {
            this.trap = trap;
        }
    }
}