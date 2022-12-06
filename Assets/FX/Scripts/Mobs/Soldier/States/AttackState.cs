using UnityEngine;

namespace SoldierNamespace
{
    public class AttackState : State
    {

        private Soldier soldier;
        private Target soldierTarget;
        public AttackState(Soldier soldier) : base(soldier)
        {
            this.type = STATE.ATTACK;
            this.soldier = soldier;
        }

        override public void OnStateEnter()
        {
            soldierTarget = soldier.target;
            soldier.SetIdleFightAnimation();
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
            if (!soldier.IsTargetInSight() || soldier.target.IsDead)
            {
                soldier.SetState(new FollowingState(soldier));
            }
        }
    }
}