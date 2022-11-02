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
    }

    override public void OnStateEnter()
    {
        digger.Job.Execute();
    }

    override public void OnStateExit()
    {
    }

}