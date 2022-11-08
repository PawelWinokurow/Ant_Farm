using UnityEngine;

public class GoToState : State
{

    private Worker worker;
    private Job job;
    private int MOVEMENT_SPEED = 5;


    public GoToState(Worker worker) : base(worker)
    {
        this.Type = STATE.GOTO;
        this.worker = worker;
    }

    override public void OnStateEnter()
    {
        job = worker.Job;
    }
    override public void OnStateExit()
    {
        worker.RemovePath();
    }

    override public void CancelJob()
    {
        if (worker.CarryingWeight == 0)
        {
            job.Cancel();
        }
    }

    public override void Tick()
    {
        if (worker.HasPath)
        {
            worker.Animation();
            worker.Move(MOVEMENT_SPEED);
        }
        else
        {
            if (worker.Job.Type == JobType.DIG || worker.Job.Type == JobType.FILL)
            {
                worker.SetState(new BuildState(worker));
            }
            else if (worker.Job.Type == JobType.CARRYING)
            {
                var job = (CarrierJob)(worker.Job);
                if (job.Direction == Direction.COLLECTING)
                {
                    worker.SetState(new LoadingState(worker));
                }
                else if (job.Direction == Direction.STORAGE)
                {
                    worker.SetState(new UnloadingState(worker));

                }
            }
        }
    }

}