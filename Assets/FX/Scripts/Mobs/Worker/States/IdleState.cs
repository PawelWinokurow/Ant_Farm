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
        worker.Animation();
        if (worker.HasPath)
        {
            worker.Move(MOVEMENT_SPEED);
            if (worker.Path.WayPoints.Count == 1)
            {
                worker.ExpandRandomWalk();
            }
        }
        else
        {
            worker.SetRandomWalk();
        }
    }

    override public void OnStateEnter()
    {
        worker.RemovePath();
        worker.SetRunAnimation();
    }
    override public void OnStateExit()
    {
    }

    override public void CancelJob()
    {

    }
}