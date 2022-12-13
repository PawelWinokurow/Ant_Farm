using UnityEngine;

namespace GobberNamespace
{
    public class AttackState : State
    {

        private Gobber gobber;
        private Target gobberTarget;
        public AttackState(Gobber gobber) : base(gobber)
        {
            this.type = STATE.ATTACK;
            this.gobber = gobber;
        }

        override public void OnStateEnter()
        {
            gobber.SetIdleFightAnimation();
            gobberTarget = gobber.target;
            gobber.animator.ResetAttack();
        }
        override public void OnStateExit()
        {
            gobber.RemovePath();
        }

        override public void CancelJob()
        {
            gobber.SetTarget(null);
        }

        public override void Tick()
        {
            if (!gobber.IsTargetInSight() || gobber.target.IsDead)
            {
                gobber.SetState(new FollowingState(gobber));
            }
        }
    }
}