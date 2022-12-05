using UnityEngine;
namespace ScorpionNamespace
{

    public class PatrolState : State
    {

        private Scorpion scorpion;
        private ScorpionSettings scorpionSettings = Settings.Instance.scorpionSettings;

        public PatrolState(Scorpion scorpion) : base(scorpion)
        {
            this.type = STATE.PATROL;
            this.scorpion = scorpion;
            scorpion.path = null;
        }
        public override void Tick()
        {
            var target = scorpion.SearchTarget();
            if (target != null)
            {
                scorpion.SetTarget(target);
                scorpion.SetState(new FollowingState(scorpion));
            }
            else if (scorpion.HasPath)
            {
                scorpion.Move();
                if (scorpion.path.wayPoints.Count == 1)
                {
                    scorpion.ExpandRandomWalk();
                }
            }
        }

        override public void OnStateEnter()
        {
            scorpion.movementSpeed = scorpionSettings.PATROL_MOVEMENT_SPEED;
            scorpion.RemovePath();
            scorpion.SetRunAnimation();
            scorpion.SetRandomWalk();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {

        }
    }
}