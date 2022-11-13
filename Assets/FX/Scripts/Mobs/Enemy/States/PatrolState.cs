using UnityEngine;

public class PatrolState : State
{

    private Enemy enemy;
    private int MOVEMENT_SPEED = 5;

    public PatrolState(Enemy enemy) : base(enemy)
    {
        this.type = STATE.PATROL;
        this.enemy = enemy;
        enemy.path = null;
    }
    public override void Tick()
    {
        enemy.Animation();
        var target = enemy.SearchTarget();
        if (target != null)
        {
            enemy.SetTarget(target);
            enemy.SetState(new FollowingState(enemy));
        }
        else if (enemy.HasPath)
        {
            enemy.Move(MOVEMENT_SPEED);
            if (enemy.path.wayPoints.Count == 1)
            {
                enemy.ExpandRandomWalk();
            }
        }
    }

    override public void OnStateEnter()
    {
        enemy.RemovePath();
        enemy.SetRunAnimation();
        enemy.SetRandomWalk();
    }
    override public void OnStateExit()
    {
    }

    override public void CancelJob()
    {

    }
}