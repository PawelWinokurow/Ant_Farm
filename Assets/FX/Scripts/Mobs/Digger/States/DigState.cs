using UnityEngine;

public class DigState : State
{
    private Digger digger;

    public DigState(Digger digger) : base(digger)
    {
        this.Type = STATE.DIG;
        this.digger = digger;
    }

    public override void Tick()
    {
        digger.ExecuteAssignment();
        digger.SetState(new IdleState(digger));
    }

    override public void OnStateEnter()
    {
    }

    override public void OnStateExit()
    {
    }

}