using UnityEngine;

public class AttackState : State
{

    private Enemy enemy;
    private Job job;
    private int MOVEMENT_SPEED = 10;


    public AttackState(Enemy enemy) : base(enemy)
    {
        this.type = STATE.ATTACK;
        this.enemy = enemy;
    }

    override public void OnStateEnter()
    {
        enemy.SetRunAtackAnimation();
        job = enemy.job;
    }
    override public void OnStateExit()
    {
        enemy.RemovePath();
    }

    override public void CancelJob()
    {
        enemy.job.Cancel();
        enemy.job = null;
    }


    public override void Tick()
    {
        if (enemy.HasPath)
        {
            enemy.Animation();
            enemy.Move(MOVEMENT_SPEED);
        }
        else
        {
            enemy.Rerouting();
        }
        enemy.Hit();
    }

}