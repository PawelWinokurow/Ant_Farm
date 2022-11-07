using UnityEngine;

public class DigState : State
{
    private Worker worker;

    public DigState(Worker worker) : base(worker)
    {
        this.Type = STATE.DIG;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.AntAnimator.Idle();
    }

    override public void OnStateEnter()
    {
        worker.Job.Execute();
    }

    override public void OnStateExit()
    {

    }

    override public void CancelJob()
    {

    }

}