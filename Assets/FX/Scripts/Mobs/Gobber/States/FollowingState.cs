using UnityEngine;

namespace GobberNamespace
{
    public class FollowingState : State
    {

        private Gobber gobber;
        private GobberSettings gobberSettings = Settings.Instance.gobberSettings;

        public FollowingState(Gobber gobber) : base(gobber)
        {
            this.type = STATE.GOTO;
            this.gobber = gobber;
        }

        override public void OnStateEnter()
        {
            gobber.movementSpeed = gobberSettings.FOLLOWING_MOVEMENT_SPEED;
            gobber.SetRunAnimation();
        }
        override public void OnStateExit()
        {
            gobber.RemovePath();
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            if (gobber.target.mob.currentState.type == STATE.DEAD)
            {
                gobber.SetState(new PatrolState(gobber));
            }
            else if (gobber.IsTargetInSight())
            {
                gobber.SetState(new AttackState(gobber));
            }
            else if (gobber.target.hex != gobber.target.mob.currentHex && !gobber.IsTargetInSight())
            {
                gobber.Rerouting();
            }
            else if (gobber.HasPath)
            {
                gobber.Move();
            }
        }
    }
}