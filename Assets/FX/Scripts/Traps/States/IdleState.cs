namespace TrapNamespace
{
    public class IdleState : State
    {
        public IdleState(Trap trap) : base(trap)
        {
            this.type = STATE.IDLE;
        }

        public override void Tick()
        {
            if (trap.IsTargetInSight())
            {
                trap.SetState(new AttackState(trap));
            }
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
