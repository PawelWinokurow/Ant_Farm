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
        if (digger.HasPath)
        {
            digger.AntAnimator.Run();
            digger.Move(MOVEMENT_SPEED);
        }
        else
        {
            digger.RandomWalk();
        }
    }

    override public void OnStateEnter()
    {
        digger.InitialPosition = digger.CurrentPosition;
        digger.RemovePath();
    }
    override public void OnStateExit()
    {
    }
}