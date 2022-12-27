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
            var target = trap.SearchTarget();
            if (target != null)
            {
                trap.SetTarget(target);
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
