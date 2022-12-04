using UnityEngine;

namespace GunnerNamespace
{
    public class PatrolState : State
    {

        private Gunner gunner;
        private GunnerSettings gunnerSettings = Settings.Instance.gunnerSettings;

        public PatrolState(Gunner gunner) : base(gunner)
        {
            this.type = STATE.PATROL;
            this.gunner = gunner;
            gunner.path = null;
        }
        public override void Tick()
        {
            var target = gunner.SearchTarget();
            if (target != null)
            {
                gunner.SetTarget(target);
                gunner.SetState(new FollowingState(gunner));
            }
            else if (gunner.HasPath)
            {
                gunner.Move();
                if (gunner.path.wayPoints.Count == 1)
                {
                    gunner.ExpandRandomWalk();
                }
            }
        }

        override public void OnStateEnter()
        {
            gunner.movementSpeed = gunnerSettings.PATROL_MOVEMENT_SPEED;
            gunner.RemovePath();
            gunner.SetRunAnimation();
            gunner.Animation();
            gunner.SetRandomWalk();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {

        }
    }
}