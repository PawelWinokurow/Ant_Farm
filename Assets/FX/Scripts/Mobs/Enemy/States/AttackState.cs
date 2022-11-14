using UnityEngine;

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
            enemy.accumulatedDamage += Enemy.ATTACK_STRENGTH * Time.deltaTime;
            if (enemy.animator.fOld < 14 && enemy.animator.f >= 14)
            {
                enemy.Attack();
                enemy.accumulatedDamage = 0f;
            }
        }
        else
        {
            enemy.SetState(new FollowingState(enemy));
        }
        enemy.animator.fOld = enemy.animator.f;
    }

}