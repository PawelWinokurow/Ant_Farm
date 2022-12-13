using UnityEngine;

namespace GobberNamespace
{
    public class PatrolState : State
    {

        private Gobber gobber;
        private GobberSettings gobberSettings = Settings.Instance.gobberSettings;

        public PatrolState(Gobber gobber) : base(gobber)
        {
            this.type = STATE.PATROL;
            this.gobber = gobber;
            gobber.path = null;
        }
        public override void Tick()
        {
            var target = gobber.SearchTarget();
            if (target != null)
            {
                gobber.SetTarget(target);
                gobber.SetState(new FollowingState(gobber));
            }
            else if (gobber.HasPath)
            {
                gobber.Move();
                if (gobber.path.wayPoints.Count == 1)
                {
                    gobber.ExpandRandomWalk();
                }
            }
        }

        override public void OnStateEnter()
        {
            gobber.movementSpeed = gobberSettings.PATROL_MOVEMENT_SPEED;
            gobber.RemovePath();
            gobber.SetRunAnimation();
            gobber.SetRandomWalk();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {

        }
    }
}