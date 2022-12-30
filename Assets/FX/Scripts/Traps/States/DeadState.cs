namespace TrapNamespace
{
    public class DeadState : State
    {
        private Trap trap;

        public DeadState(Trap trap)
        {
            this.type = STATE.DEAD;
            this.trap = trap;
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
