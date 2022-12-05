using UnityEngine;

namespace ScorpionNamespace
{
    public class AttackState : State
    {

        private Scorpion scorpion;
        private Target scorpionTarget;
        public AttackState(Scorpion scorpion) : base(scorpion)
        {
            this.type = STATE.ATTACK;
            this.scorpion = scorpion;
        }

        override public void OnStateEnter()
        {
            scorpionTarget = scorpion.target;
            scorpion.SetIdleFightAnimation();
            scorpion.animator.ResetAttack();
        }
        override public void OnStateExit()
        {
            scorpion.RemovePath();
        }

        override public void CancelJob()
        {
            scorpion.SetTarget(null);
        }

        public override void Tick()
        {
            if (!scorpion.IsTargetInNeighbourhood())
            {
                scorpion.SetState(new FollowingState(scorpion));
            }
            if (scorpion.target.mob.currentState.type == STATE.DEAD)
            {
                scorpion.SetState(new PatrolState(scorpion));
            }
        }

    }
}