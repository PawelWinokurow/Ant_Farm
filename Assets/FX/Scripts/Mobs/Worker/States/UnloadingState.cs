using UnityEngine;
public class UnloadingState : State
{
    public Worker worker;
    public bool IsDone;
    private CarrierJob job;

    public UnloadingState(Worker worker) : base(worker)
    {
        Type = STATE.UNLOADING;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.AntAnimator.Idle();
        if (IsDone)
        {
            worker.SetPath(worker.Pathfinder.FindPath(worker.Position, job.Destination, true));
            worker.SetRunAnimation();
            worker.SetState(new GoToState(worker));
        }
        else if (!IsDone)
        {
            worker.SurfaceOperations.Unloading(job.StorageHexagon, this, job);
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
        if (worker.CarryingWeight == 0)
        {
            job.Cancel();
        }
    }
    override public void OnStateEnter()
    {
        IsDone = false;
        job = (CarrierJob)worker.Job;
        worker.SetIdleAnimation();
    }

    override public void OnStateExit()
    {

    }

}