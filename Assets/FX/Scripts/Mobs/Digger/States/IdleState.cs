using UnityEngine;

public class IdleState : State
{

    private Digger digger;
    private int MOVEMENT_SPEED = 2;

    public IdleState(Digger digger) : base(digger)
    {
        this.Type = STATE.IDLE;
        this.digger = digger;
    }
    public override void Tick()
    {
        if (digger.HasPath)
        {
            digger.Move(MOVEMENT_SPEED);
        }
    }

    override public void OnStateEnter()
    {
        digger.InitialPosition = digger.CurrentPosition;
    }
    override public void OnStateExit()
    {
        digger.ResetWaypoints();
    }
}