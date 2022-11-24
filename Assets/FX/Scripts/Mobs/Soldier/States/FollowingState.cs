using UnityEngine;

namespace SoldierNamespace
{
    public class FollowingState : State
    {

        private Soldier soldier;
        private int MOVEMENT_SPEED = 10;


        public FollowingState(Soldier soldier) : base(soldier)
        {
            this.type = STATE.GOTO;
            this.soldier = soldier;
        }

        override public void OnStateEnter()
        {
            soldier.SetRunAnimation();
            soldier.Animation();
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
                soldier.Move(MOVEMENT_SPEED);
            }
        }
    }
}