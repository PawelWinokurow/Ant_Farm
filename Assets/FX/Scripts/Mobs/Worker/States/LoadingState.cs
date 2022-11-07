using UnityEngine;
public class LoadingState : State
{
    private Worker worker;

    public LoadingState(Worker worker) : base(worker)
    {
        this.Type = STATE.LOADING;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.AntAnimator.Idle();
    }

    override public void OnStateEnter()
    {
        // surface.StartJobExecution(job.Hex, worker);

        // worker.Job.Execute();
    }

    override public void OnStateExit()
    {

    }

}