namespace FighterNamespace
{
    public class AttackState : State
    {

        private Fighter fighter;
        private Target fighterTarget;
        public AttackState(Fighter fighter)
        {
            this.type = STATE.ATTACK;
            this.fighter = fighter;
        }

        override public void OnStateEnter()
        {
            fighter.SetIdleFightAnimation();
            fighterTarget = fighter.target;
        }
        override public void OnStateExit()
        {
            fighter.RemovePath();
        }

        override public void CancelJob()
        {
            fighter.SetTarget(null);
        }

        public override void Tick()
        {
            if (!fighter.IsTargetInSight() || fighter.target.IsDead)
            {
                fighter.SetState(new FollowingState(fighter));
            }
        }
    }
}
