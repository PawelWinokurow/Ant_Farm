namespace ScorpionNamespace
{

    public class FollowingState : State
    {

        private Scorpion scorpion;
        private GameSettings gameSettings = Settings.Instance.gameSettings;
        private ScorpionSettings scorpionSettings = Settings.Instance.scorpionSettings;

        public FollowingState(Scorpion scorpion) : base(scorpion)
        {
            type = STATE.GOTO;
            this.scorpion = scorpion;
        }

        override public void OnStateEnter()
        {
            scorpion.SetRunAnimation();
            scorpion.accessMask = gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_SOIL;
            scorpion.movementSpeed = scorpionSettings.FOLLOWING_MOVEMENT_SPEED;
        }
        override public void OnStateExit()
        {
            scorpion.RemovePath();
            scorpion.accessMask = gameSettings.ACCESS_MASK_FLOOR;
        }

        override public void CancelJob()
        {
        }

        public override void Tick()
        {
            var target = scorpion.SearchTarget();
            if (target != null && target.mob != scorpion.target.mob)
            {
                scorpion.SetTarget(target);
            }
            if (scorpion.target.mob.currentState.type == STATE.DEAD)
            {
                scorpion.SetState(new PatrolState(scorpion));
            }
            else if (scorpion.IsTargetInSight() && scorpion.currentHex.type != HexType.SOIL)
            {
                scorpion.SetState(new AttackState(scorpion));
            }
            else if (scorpion.target.hex != scorpion.target.mob.currentHex && !scorpion.IsTargetInSight())
            {
                scorpion.Rerouting();
            }

            else if (scorpion.HasPath)
            {
                scorpion.Move();
            }
        }

    }
}