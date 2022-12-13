using UnityEngine;

namespace FighterNamespace
{
    public class PatrolState : State
    {

        private Fighter fighter;

        public PatrolState(Fighter fighter) : base(fighter)
        {
            this.type = STATE.PATROL;
            this.fighter = fighter;
            fighter.path = null;
        }
        public override void Tick()
        {
            var target = fighter.SearchTarget();
            if (target != null)
            {
                fighter.SetTarget(target);
                fighter.SetState(new FollowingState(fighter));
            }
            else if (fighter.HasPath)
            {
                fighter.Move();
                if (fighter.path.wayPoints.Count == 1)
                {
                    fighter.ExpandRandomWalk();
                }
            }
        }

        override public void OnStateEnter()
        {
            fighter.movementSpeed = fighter.mobSettings.PATROL_MOVEMENT_SPEED;
            fighter.RemovePath();
            fighter.SetRunAnimation();
            fighter.SetRandomWalk();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {

        }
    }
}
