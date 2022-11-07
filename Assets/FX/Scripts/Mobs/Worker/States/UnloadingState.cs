using UnityEngine;
public class UnloadingState : State
{
    private Worker worker;

    public UnloadingState(Worker worker) : base(worker)
    {
        this.Type = STATE.UNLOADING;
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