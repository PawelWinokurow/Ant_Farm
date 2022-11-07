using UnityEngine;

public class BuildState : State
{
    private Worker Worker;
    public bool IsDone;
    private WorkerJob job;
    public BuildState(Worker worker) : base(worker)
    {
        this.Type = STATE.BUILD;
        this.Worker = worker;
    }

    public override void Tick()
    {
        Worker.Animation();
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
        job = (WorkerJob)Worker.Job;
        Worker.SurfaceOperations.Build(job);
        Worker.SetIdleAnimation();
    }

    override public void OnStateExit()
    {

    }

}