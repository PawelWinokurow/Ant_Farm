using UnityEngine;

public class IdleState : State
{

    private Digger digger;
    private int MOVEMENT_SPEED = 5;

    public IdleState(Digger digger) : base(digger)
    {
        this.Type = STATE.IDLE;
        this.digger = digger;
    }
    public override void Tick()
    {
        if (digger.HasPath && digger.Path.HasWaypoints)
        {
            digger.AntAnimator.Run();
            digger.Move(MOVEMENT_SPEED);
        }
        else
        {
            digger.SetRandomWalk();
        }
    }

    override public void OnStateEnter()
    {
        digger.RemovePath();
    }
    override public void OnStateExit()
    {
    }
}