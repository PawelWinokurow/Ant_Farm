using UnityEngine;

public class DeadState : State
{
    // private Mob mob;
    public DeadState(Mob mob) : base(mob)
    {
        this.type = STATE.DEAD;
        // this.mob = mob;
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