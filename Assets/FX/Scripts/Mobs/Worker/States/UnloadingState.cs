using UnityEngine;
public class UnloadingState : State
{
    public Worker worker;
    public bool IsDone;
    private CarrierJob job;

    public UnloadingState(Worker worker) : base(worker)
    {
        type = STATE.UNLOADING;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.animator.Idle();
        if (IsDone)
        {
            worker.SetPath(worker.pathfinder.FindPath(worker.position, job.destination, Worker.ACCESS_MASK, true));
            worker.SetRunAnimation();
            worker.SetState(new GoToState(worker));
        }
        else if (!IsDone)
        {
            worker.surfaceOperations.Unloading(job.storageHexagon, this, job);
        }
    }

    public void Done()
    {

        if (job.collectingHexagon.floorHexagon.type == HEX_TYPE.FOOD)
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
        if (worker.carryingWeight == 0)
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