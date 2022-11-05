using UnityEngine;

public class GoToState : State
{

    private Digger digger;
    private int MOVEMENT_SPEED = 5;


    public GoToState(Digger digger) : base(digger)
    {
        this.Type = STATE.GOTO;
        this.digger = digger;
    }

    override public void OnStateEnter()
    {
    }
    override public void OnStateExit()
    {
        digger.RemovePath();
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
            digger.SetState(new DigState(digger));
        }
    }

}