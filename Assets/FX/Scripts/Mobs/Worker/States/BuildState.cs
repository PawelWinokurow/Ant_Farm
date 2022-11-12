using UnityEngine;

public class BuildState : State
{
    private Worker worker;
    public bool IsDone;
    private WorkerJob job;
    public BuildState(Worker worker) : base(worker)
    {
        this.Type = STATE.BUILD;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.Animation();
    }

    public void Done()
    {
        IsDone = true;
    }

    override public void CancelJob()
    {
        job.Cancel();
    }

    override public void OnStateEnter()
    {
        IsDone = false;
        job = (WorkerJob)worker.Job;
        worker.SurfaceOperations.Build(job);
        worker.SetIdleAnimation();
    }

    override public void OnStateExit()
    {

    }

}