using UnityEngine;

namespace EnemyNamespace
{
    public class AttackState : State
    {

        private Enemy enemy;
        private EnemyTarget enemyTarget;
        public AttackState(Enemy enemy) : base(enemy)
        {
            this.type = STATE.ATTACK;
            this.enemy = enemy;
        }

        override public void OnStateEnter()
        {
            // enemy.RemoveDigFX();
            enemy.SetIdleFightAnimation();
            enemyTarget = enemy.target;
            enemy.Animation();
            enemy.animator.ResetAttack();
        }
        override public void OnStateExit()
        {
            enemy.RemovePath();
        }

        override public void CancelJob()
        {
            enemy.SetTarget(null);
        }

        public override void Tick()
        {
            if (!enemy.IsTargetInNeighbourhood())
            {
                enemy.SetState(new FollowingState(enemy));
            }
            if (enemy.target.mob.currentState.type == STATE.DEAD)
            {
                enemy.SetState(new PatrolState(enemy));
            }
        }

    }
}