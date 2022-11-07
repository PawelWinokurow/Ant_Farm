using UnityEngine;
public class LoadingState : State
{
    public Worker worker;
    public bool IsDone;
    private CarrierJob job;

    public LoadingState(Worker worker) : base(worker)
    {
        Type = STATE.LOADING;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.Animation();
        if (IsDone)
        {
            job.SwapDestination();
            worker.SetPath(worker.Pathfinder.FindPath(worker.Position, job.Destination, true));
            worker.SetRunFoodAnimation();
            worker.SetState(new GoToState(worker));
        }
        else if (!IsDone)
        {
            worker.SurfaceOperations.Loading(job.CollectingHexagon, this, job);
        }
    }

    public void Done()
    {
        IsDone = true;
    }

    override public void CancelJob()

    {
        if (worker.CarryingWeight != 0)
        {
            worker.SetPath(worker.Pathfinder.FindPath(worker.Position, job.StorageHexagon.Position, true));
            worker.SetRunAnimation();
            worker.SetState(new GoToState(worker));
        }
        else
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