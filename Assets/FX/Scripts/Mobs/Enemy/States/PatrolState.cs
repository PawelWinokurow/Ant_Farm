using UnityEngine;

public class PatrolState : State
{

    private Enemy enemy;
    private int MOVEMENT_SPEED = 5;

    public PatrolState(Enemy enemy) : base(enemy)
    {
        this.type = STATE.IDLE;
        this.enemy = enemy;
        enemy.path = null;
    }
    public override void Tick()
    {
        enemy.Animation();
        if (enemy.HasPath)
        {
            enemy.Move(MOVEMENT_SPEED);
            if (enemy.path.wayPoints.Count == 1)
            {
                enemy.ExpandRandomWalk();
            }
        }
        else
        {
            enemy.SetRandomWalk();
        }
        enemy.SearchTarget();
    }

    override public void OnStateEnter()
    {
        enemy.RemovePath();
        enemy.SetRunAnimation();
    }
    override public void OnStateExit()
    {
    }

    override public void CancelJob()
    {

    }
}