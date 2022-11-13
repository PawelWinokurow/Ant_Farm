using UnityEngine;

public class FollowingState : State
{

    private Enemy enemy;
    private EnemyTarget enemyTarget;
    private int MOVEMENT_SPEED = 10;


    public FollowingState(Enemy enemy) : base(enemy)
    {
        this.type = STATE.GOTO;
        this.enemy = enemy;
    }

    override public void OnStateEnter()
    {
        enemy.SetRunAnimation();
        enemyTarget = enemy.target;
    }
    override public void OnStateExit()
    {
        enemy.RemovePath();
    }

    override public void CancelJob()
    {
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
            enemy.SetState(new AttackState(enemy));
        }
    }

}