using UnityEngine;

namespace EnemyNamespace
{

    public class FollowingState : State
    {

        private Enemy enemy;
        private int MOVEMENT_SPEED = 10;


        public FollowingState(Enemy enemy) : base(enemy)
        {
            this.type = STATE.GOTO;
            this.enemy = enemy;
        }

        override public void OnStateEnter()
        {
            enemy.SetRunAnimation();
            enemy.Animation();
        }
        override public void OnStateExit()
        {
            enemy.RemovePath();
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            if (enemy.target.mob.currentState.type == STATE.DEAD)
            {
                enemy.SetState(new PatrolState(enemy));
            }
            else if (enemy.IsTargetInNeighbourhood())
            {
                enemy.SetState(new AttackState(enemy));
            }
            else if (enemy.target.hex != enemy.target.mob.currentHex && !enemy.IsTargetInNeighbourhood())
            {
                enemy.Rerouting();
            }
            else if (enemy.HasPath)
            {
                enemy.Move(MOVEMENT_SPEED);
            }
        }

    }
}