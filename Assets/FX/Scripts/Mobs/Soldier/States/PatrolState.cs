using UnityEngine;

namespace SoldierNamespace
{
    public class PatrolState : State
    {

        private Soldier soldier;
        private SoldierSettings soldierSettings = Settings.Instance.soldierSettings;

        public PatrolState(Soldier soldier) : base(soldier)
        {
            this.type = STATE.PATROL;
            this.soldier = soldier;
            soldier.path = null;
        }
        public override void Tick()
        {
            var target = soldier.SearchTarget();
            if (target != null)
            {
                soldier.SetTarget(target);
                soldier.SetState(new FollowingState(soldier));
            }
            else if (soldier.HasPath)
            {
                soldier.Move();
                if (soldier.path.wayPoints.Count == 1)
                {
                    soldier.ExpandRandomWalk();
                }
            }
        }

        override public void OnStateEnter()
        {
            soldier.movementSpeed = soldierSettings.PATROL_MOVEMENT_SPEED;
            soldier.RemovePath();
            soldier.SetRunAnimation();
            soldier.Animation();
            soldier.SetRandomWalk();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {

        }
    }
}