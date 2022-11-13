using System.Linq;
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
        enemy.SetIdleFightAnimation();
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
        if (enemy.currentHex.vertex.neighbours.Select(vertex => vertex.id).Contains(enemy.target.mob.currentHex.id))
        {
            enemy.Hit();
        }
        else
        {
            enemy.Rerouting();
            enemy.SetState(new FollowingState(enemy));
        }
    }

}