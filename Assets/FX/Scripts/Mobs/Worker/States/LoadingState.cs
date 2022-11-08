using UnityEngine;
public class LoadingState : State
{
    public Worker Worker;
    public bool IsDone;
    private CarrierJob job;

    public LoadingState(Worker worker) : base(worker)
    {
        Type = STATE.LOADING;
        Worker = worker;
    }

    public override void Tick()
    {
        if (IsDone)
        {
            job.SwapDestination();
            Worker.SetPath(Worker.Pathfinder.FindPath(Worker.Position, job.Destination, true));
            Worker.SetRunFoodAnimation();
            Worker.SetState(new GoToState(Worker));
        }
        else if (!IsDone)
        {
            Worker.SurfaceOperations.Loading(job.CollectingHexagon, this, job);
        }
    }

    public void Done()
    {
        IsDone = true;
    }

    override public void CancelJob()

    {
        if (Worker.CarryingWeight != 0)
        {
            Worker.SetPath(Worker.Pathfinder.FindPath(Worker.Position, job.StorageHexagon.Position, true));
            Worker.SetRunAnimation();
            Worker.SetState(new GoToState(Worker));
        }
        else
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