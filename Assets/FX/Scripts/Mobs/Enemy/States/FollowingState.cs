using UnityEngine;

namespace EnemyNamespace
{

    public class FollowingState : State
    {

        private Enemy enemy;
        private GameSettings gameSettings = Settings.Instance.gameSettings;
        private ScorpionSettings scorpionSettings = Settings.Instance.scorpionSettings;

        public FollowingState(Enemy enemy) : base(enemy)
        {
            type = STATE.GOTO;
            this.enemy = enemy;
        }

        override public void OnStateEnter()
        {
            enemy.SetRunAnimation();
            enemy.Animation();
            enemy.accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_SOIL;
            enemy.movementSpeed = scorpionSettings.FOLLOWING_MOVEMENT_SPEED;
        }
        override public void OnStateExit()
        {
            enemy.RemovePath();
            enemy.accessMask = gameSettings.ACCESS_MASK_FLOOR;
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            var target = enemy.SearchTarget();
            if (target != null && target.mob != enemy.target.mob)
            {
                enemy.SetTarget(target);
            }
            if (enemy.target.mob.currentState.type == STATE.DEAD)
            {
                enemy.SetState(new PatrolState(enemy));
            }
            else if (enemy.IsTargetInNeighbourhood() && enemy.currentHex.type != HexType.SOIL)
            {
                enemy.SetState(new AttackState(enemy));
            }
            else if (enemy.target.hex != enemy.target.mob.currentHex && !enemy.IsTargetInNeighbourhood())
            {
                enemy.Rerouting();
            }

            else if (enemy.HasPath)
            {
                enemy.Move();
            }
        }

    }
}