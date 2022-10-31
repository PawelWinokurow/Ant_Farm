using UnityEngine;

public class IdleState : State
{

    private Digger digger;

    public IdleState(Digger digger) : base(digger)
    {
        this.Type = STATE.IDLE;
        this.digger = digger;
    }
    public override void Tick()
    {
        Debug.Log("Tick Idle State");
    }

    override public void OnStateEnter()
    {
        Debug.Log("OnStateEnter Idle State");
    }
    override public void OnStateExit()
    {
        Debug.Log("OnStateExit Idle State");
    }
}