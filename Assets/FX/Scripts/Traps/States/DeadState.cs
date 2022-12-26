namespace TrapNamespace
{
    public class DeadState : State
    {
        public DeadState(Trap trap) : base(trap)
        {
            this.type = STATE.DEAD;
        }

        public override void Tick()
        {
        }

        override public void CancelJob()
        {
        }

        override public void OnStateEnter()
        {
            trap.SetIdleAnimation();
        }

        override public void OnStateExit()
        {
        }
    }
}
