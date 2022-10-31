using UnityEngine;

public class GoToState : State
{

    private Digger digger;

    public GoToState(Digger digger) : base(digger)
    {
        this.Type = STATE.GOTO;
        this.digger = digger;
    }

    override public void OnStateEnter()
    {
        Debug.Log("OnStateEnter GoTo State");
        digger.ResetMovement();
    }
    override public void OnStateExit()
    {
        Debug.Log("OnStateExit GoTo State");
        digger.ResetWaypoints();
    }

    public override void Tick()
    {
        if (!digger.IsGoalReached)
        {
            digger.Move();
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