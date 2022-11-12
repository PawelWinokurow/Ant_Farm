using UnityEngine;
public class LoadingState : State
{
    public Worker worker;
    public bool IsDone;
    private CarrierJob job;

    public LoadingState(Worker worker) : base(worker)
    {
        type = STATE.LOADING;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.Animation();
        if (IsDone)
        {
            job.SwapDestination();
            worker.SetPath(worker.pathfinder.FindPath(worker.position, job.destination, true));
            worker.SetRunFoodAnimation();
            worker.SetState(new GoToState(worker));
        }
        else if (!IsDone)
        {
            worker.surfaceOperations.Loading(job.collectingHexagon, this, job);
        }
    }

    public void Done()
    {
        IsDone = true;
    }

    override public void CancelJob()

    {
        if (worker.CARRYING_WEIGHT != 0)
        {
            worker.SetPath(worker.pathfinder.FindPath(worker.position, job.storageHexagon.position, true));
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
        job = (CarrierJob)worker.job;
        worker.SetIdleAnimation();
    }

    override public void OnStateExit()
    {

    }

}