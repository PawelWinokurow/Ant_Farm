using UnityEngine;
namespace EnemyNamespace
{

    public class PatrolState : State
    {

        private Enemy enemy;
        private ScorpionSettings scorpionSettings = Settings.Instance.scorpionSettings;

        public PatrolState(Enemy enemy) : base(enemy)
        {
            this.type = STATE.PATROL;
            this.enemy = enemy;
            enemy.path = null;
        }
        public override void Tick()
        {
            var target = enemy.SearchTarget();
            if (target != null)
            {
                enemy.SetTarget(target);
                enemy.SetState(new FollowingState(enemy));
            }
            else if (enemy.HasPath)
            {
                enemy.Move();
                if (enemy.path.wayPoints.Count == 1)
                {
                    enemy.ExpandRandomWalk();
                }
            }
        }

        override public void OnStateEnter()
        {
            enemy.movementSpeed = scorpionSettings.PATROL_MOVEMENT_SPEED;
            enemy.RemovePath();
            enemy.SetRunAnimation();
            enemy.Animation();
            enemy.SetRandomWalk();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {

        }
    }
}