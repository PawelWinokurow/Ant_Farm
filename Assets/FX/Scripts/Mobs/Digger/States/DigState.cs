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
        Debug.Log("Tick Dig State");
        digger.ExecuteAssignment();
        digger.SetState(new IdleState(digger));
    }

    override public void OnStateEnter()
    {
        Debug.Log("OnStateEnter Dig State");
    }

    override public void OnStateExit()
    {
        Debug.Log("OnStateExit Dig State");
    }

}