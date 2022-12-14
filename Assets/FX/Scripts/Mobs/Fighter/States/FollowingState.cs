namespace FighterNamespace
{
    public class FollowingState : State
    {

        private Fighter fighter;

        public FollowingState(Fighter fighter) : base(fighter)
        {
            this.type = STATE.GOTO;
            this.fighter = fighter;
        }

        override public void OnStateEnter()
        {
            fighter.movementSpeed = fighter.mobSettings.FOLLOWING_MOVEMENT_SPEED;
            fighter.accessMask = fighter.mobSettings.FOLLOWING_ACCESS_MASK;
            fighter.SetRunAnimation();
        }
        override public void OnStateExit()
        {
            fighter.RemovePath();
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            if (fighter.target.mob.currentState.type == STATE.DEAD)
            {
                fighter.SetState(new PatrolState(fighter));
            }
            else if (fighter.IsTargetInSight())
            {
                fighter.SetState(new AttackState(fighter));
            }
            else if (fighter.target.hex != fighter.target.mob.currentHex && !fighter.IsTargetInSight())
            {
                fighter.Rerouting();
            }
            else if (fighter.HasPath)
            {
                fighter.Move();
            }
        }
    }
}