using UnityEngine;

namespace GunnerNamespace
{
    public class FollowingState : State
    {

        private Gunner gunner;
        private GunnerSettings gunnerSettings = Settings.Instance.gunnerSettings;

        public FollowingState(Gunner gunner) : base(gunner)
        {
            this.type = STATE.GOTO;
            this.gunner = gunner;
        }

        override public void OnStateEnter()
        {
            gunner.movementSpeed = gunnerSettings.FOLLOWING_MOVEMENT_SPEED;
            gunner.SetRunAnimation();
            gunner.Animation();
        }
        override public void OnStateExit()
        {
            gunner.RemovePath();
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            if (gunner.target.mob.currentState.type == STATE.DEAD)
            {
                gunner.SetState(new PatrolState(gunner));
            }
            else if (gunner.IsTargetInSight())
            {
                gunner.SetState(new AttackState(gunner));
            }
            else if (gunner.target.hex != gunner.target.mob.currentHex && !gunner.IsTargetInSight())
            {
                gunner.Rerouting();
            }
            else if (gunner.HasPath)
            {
                gunner.Move();
            }
        }
    }
}