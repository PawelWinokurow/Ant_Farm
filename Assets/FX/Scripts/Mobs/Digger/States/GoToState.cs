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
        digger.ResetWaypoints();
    }

    public override void Tick()
    {
        if (digger.HasPath)
        {
            digger.Move(MOVEMENT_SPEED);
        }
        else if (digger.HasAsignment)
        {
            if (digger.Job.Assignment.Type == AssignmentType.DIG || digger.Job.Assignment.Type == AssignmentType.FILL)
            {
                digger.SetState(new DigState(digger));
            }
        }
        else
        {
            digger.SetState(new IdleState(digger));
        }
    }




}