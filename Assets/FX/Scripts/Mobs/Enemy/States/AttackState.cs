using UnityEngine;

public class AttackState : State
{

    private Enemy enemy;
    private Job job;

    public AttackState(Enemy enemy) : base(enemy)
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
    { }


    public override void Tick()
    {

    }

}