namespace TrapNamespace
{
    public class AttackState : State
    {
        public AttackState(Trap trap) : base(trap)
        {
            this.type = STATE.ATTACK;
            this.trap = trap;
        }

        override public void OnStateEnter()
        {
            trap.SetIdleFightAnimation();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            trap.Rotation();
            if (!trap.IsTargetInSight())
            {
                trap.SetState(new IdleState(trap));
            }
        }
    }
}
