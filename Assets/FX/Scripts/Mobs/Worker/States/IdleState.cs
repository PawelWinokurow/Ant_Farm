using UnityEngine;

public class IdleState : State
{

    private Worker worker;
    private int MOVEMENT_SPEED = 5;

    public IdleState(Worker worker) : base(worker)
    {
        this.Type = STATE.IDLE;
        this.worker = worker;
        worker.Path = null;
    }
    public override void Tick()
    {
        if (worker.HasPath)
        {
            worker.Move(MOVEMENT_SPEED);
        }
        else
        {
            worker.SetRandomWalk();
        }
    }

    override public void OnStateEnter()
    {
        worker.RemovePath();
        if (worker.AntAnimator != null)
        {
            worker.SetRunAnimation();
        }
    }
    override public void OnStateExit()
    {
    }

    override public void CancelJob()
    {

    }
}