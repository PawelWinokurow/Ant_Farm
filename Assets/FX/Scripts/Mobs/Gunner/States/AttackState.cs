using UnityEngine;

namespace GunnerNamespace
{
    public class AttackState : State
    {

        private Gunner gunner;
        private Target gunnerTarget;
        public AttackState(Gunner gunner) : base(gunner)
        {
            this.type = STATE.ATTACK;
            this.gunner = gunner;
        }

        override public void OnStateEnter()
        {
            gunner.SetIdleFightAnimation();
            gunnerTarget = gunner.target;
            gunner.animator.ResetAttack();
        }
        override public void OnStateExit()
        {
            gunner.RemovePath();
        }

        override public void CancelJob()
        {
            gunner.SetTarget(null);
        }

        public override void Tick()
        {
            if (!gunner.IsTargetInSight())
            {
                gunner.SetState(new FollowingState(gunner));
            }
            if (gunner.target.mob.currentState.type == STATE.DEAD)
            {
                gunner.SetState(new PatrolState(gunner));
            }
        }
    }
}