using UnityEngine;
public class DeadState : State
{
    public DeadState(Mob mob) : base(mob)
    {
        this.type = STATE.DEAD;
        mob.SetIdleAnimation();
    }

    public override void Tick()
    {
    }

    override public void CancelJob()
    {
    }

    override public void OnStateEnter()
    {
    }

    override public void OnStateExit()
    {
    }

}
