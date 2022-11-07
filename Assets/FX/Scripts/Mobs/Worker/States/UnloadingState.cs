using UnityEngine;
public class UnloadingState : State
{
    public Worker Worker;
    public bool IsDone;
    private CarrierJob job;

    public UnloadingState(Worker worker) : base(worker)
    {
        Type = STATE.UNLOADING;
        Worker = worker;
    }

    public override void Tick()
    {
        Worker.AntAnimator.Idle();
        if (IsDone)
        {
            Worker.SetPath(Worker.Pathfinder.FindPath(Worker.Position, job.Destination, true));
            Worker.SetRunAnimation();
            Worker.SetState(new GoToState(Worker));
        }
        else if (!IsDone)
        {
            Worker.SurfaceOperations.Unloading(job.StorageHexagon, this, job);
        }
    }

    public void Done()
    {

        if (job.CollectingHexagon.FloorHexagon.Type == HEX_TYPE.FOOD)
        {
            job.SwapDestination();
        }
        else
        {
            CancelJob();
        }
        IsDone = true;
    }
    override public void CancelJob()
    {
        if (Worker.CarryingWeight == 0)
        {
            job.Cancel();
        }
    }
    override public void OnStateEnter()
    {
        IsDone = false;
        job = (CarrierJob)Worker.Job;
        Worker.SetIdleAnimation();
    }

    override public void OnStateExit()
    {

    }

}