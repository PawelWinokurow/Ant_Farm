using UnityEngine;

public class AttackState : State
{

    private Enemy enemy;
    private EnemyTarget enemyTarget;
    private bool isHitMade;
    public AttackState(Enemy enemy) : base(enemy)
    {
        this.type = STATE.ATTACK;
        this.enemy = enemy;
        this.isHitMade = false;
        enemy.accumulatedDamage = 0f;
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
        if (enemy.IsTargetInNeighbourhood() && enemy.target.mob.currentState.type != STATE.DEAD)
        {
            enemy.Animation();
            enemy.accumulatedDamage += Enemy.ATTACK_STRENGTH * Time.deltaTime;
            if (enemy.animator.fOld > enemy.animator.f)
            {
                isHitMade = false;
            }
            if (!isHitMade && enemy.animator.f >= 14)
            {
                enemy.Attack();
                enemy.accumulatedDamage = 0f;
                isHitMade = true;
            }
        }
        else
        {
            enemy.SetState(new FollowingState(enemy));
        }
        enemy.animator.fOld = enemy.animator.f;
    }

}