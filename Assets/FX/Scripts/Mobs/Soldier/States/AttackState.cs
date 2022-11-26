using UnityEngine;

namespace SoldierNamespace
{
    public class AttackState : State
    {

        private Soldier soldier;
        private SoldierTarget soldierTarget;
        public AttackState(Soldier soldier) : base(soldier)
        {
            this.type = STATE.ATTACK;
            this.soldier = soldier;
        }

        override public void OnStateEnter()
        {
            soldier.SetIdleAnimation();
            soldierTarget = soldier.target;
            soldier.Animation();
            soldier.animator.ResetAttack();
        }
        override public void OnStateExit()
        {
            soldier.RemovePath();
        }

        override public void CancelJob()
        {
            soldier.SetTarget(null);
        }

        public override void Tick()
        {
            if (!soldier.IsTargetInNeighbourhood())
            {
                soldier.SetState(new FollowingState(soldier));
            }
            if (soldier.target.mob.currentState.type == STATE.DEAD)
            {
                soldier.SetState(new PatrolState(soldier));
            }
        }
    }
}