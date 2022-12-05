using UnityEngine;

namespace SoldierNamespace
{
    public class FollowingState : State
    {

        private Soldier soldier;
        private SoldierSettings soldierSettings = Settings.Instance.soldierSettings;

        public FollowingState(Soldier soldier) : base(soldier)
        {
            this.type = STATE.GOTO;
            this.soldier = soldier;
        }

        override public void OnStateEnter()
        {
            soldier.movementSpeed = soldierSettings.FOLLOWING_MOVEMENT_SPEED;
            soldier.SetRunAnimation();
        }
        override public void OnStateExit()
        {
            soldier.RemovePath();
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            if (soldier.target.mob.currentState.type == STATE.DEAD)
            {
                soldier.SetState(new PatrolState(soldier));
            }
            else if (soldier.IsTargetInNeighbourhood())
            {
                soldier.SetState(new AttackState(soldier));
            }
            else if (soldier.target.hex != soldier.target.mob.currentHex && !soldier.IsTargetInNeighbourhood())
            {
                soldier.Rerouting();
            }
            else if (soldier.HasPath)
            {
                soldier.Move();
            }
        }
    }
}