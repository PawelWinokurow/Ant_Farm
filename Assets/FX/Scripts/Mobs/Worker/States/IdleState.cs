using UnityEngine;

public class IdleState : State
{

    private Worker worker;
    private int MOVEMENT_SPEED = 5;

    public IdleState(Worker worker) : base(worker)
    {
        this.type = STATE.IDLE;
        this.worker = worker;
        worker.path = null;
    }
    public override void Tick()
    {
        if (worker.HasPath)
        {
            worker.Move(MOVEMENT_SPEED);
            if (worker.path.wayPoints.Count == 1)
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
        worker.Animation();
    }
    override public void OnStateExit()
    {
    }

    override public void CancelJob()
    {

    }
}