using UnityEngine;

public class FollowState : State
{

    private Enemy enemy;
    private Job job;
    private int MOVEMENT_SPEED = 7;


    public FollowState(Enemy enemy) : base(enemy)
    {
        this.Type = STATE.GOTO;
        this.enemy = enemy;
    }

    override public void OnStateEnter()
    {
        job = enemy.Job;
    }
    override public void OnStateExit()
    {
        enemy.RemovePath();
    }

    override public void CancelJob()
    {
        if (enemy.CarryingWeight == 0)
        {
            job.Cancel();
        }
    }

    public override void Tick()
    {
        if (enemy.HasPath)
        {
            enemy.Animation();
            enemy.Move(MOVEMENT_SPEED);
        }
    }

}