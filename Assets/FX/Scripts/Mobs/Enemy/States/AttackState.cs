public class AttackState : State
{

    private Enemy enemy;
    private EnemyTarget enemyTarget;

    public AttackState(Enemy enemy) : base(enemy)
    {
        this.type = STATE.ATTACK;
        this.enemy = enemy;
    }

    override public void OnStateEnter()
    {
        enemy.SetIdleFightAnimation();
        enemyTarget = enemy.target;
    }
    override public void OnStateExit()
    {
        enemy.RemovePath();
    }

    override public void CancelJob()
    {
        enemy.SetTarget(null);
    }


    public override void Tick()
    {
        if (enemy.IsTargetInNeighbourhood())
        {
            enemy.Animation();
            enemy.Attack();
        }
        else
        {
            enemy.SetState(new FollowingState(enemy));
        }
    }

}