using UnityEngine;

public class AttackState : State
{

    private Enemy enemy;
    private EnemyTarget enemyTarget;
    private int MOVEMENT_SPEED = 10;


    public AttackState(Enemy enemy) : base(enemy)
    {
        this.type = STATE.ATTACK;
        this.enemy = enemy;
    }

    override public void OnStateEnter()
    {
        enemy.SetRunAtackAnimation();
        enemyTarget = enemy.target;
    }
    override public void OnStateExit()
    {
        enemy.RemovePath();
    }

    override public void CancelJob()
    {
        enemyTarget.Cancel();
        enemy.target = null;
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