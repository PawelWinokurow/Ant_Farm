using UnityEngine;

public class PatrolState : State
{

    private Enemy enemy;
    private int MOVEMENT_SPEED = 5;

    public PatrolState(Enemy enemy) : base(enemy)
    {
        this.Type = STATE.IDLE;
        this.enemy = enemy;
        enemy.Path = null;
    }
    public override void Tick()
    {
        enemy.Animation();
        if (enemy.HasPath)
        {
            enemy.Move(MOVEMENT_SPEED);
            if (enemy.Path.WayPoints.Count == 1)
            {
                enemy.ExpandRandomWalk();
            }
        }
        else
        {
            enemy.SetRandomWalk();
        }
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