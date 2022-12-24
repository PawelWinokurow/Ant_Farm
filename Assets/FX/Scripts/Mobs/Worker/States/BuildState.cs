using MobNamespace;

namespace WorkerNamespace
{
    public class BuildState : State
    {
        private Worker worker;
        private WorkerJob job;
        public BuildState(Worker worker) : base(worker)
        {
            this.type = STATE.BUILD;
            this.worker = worker;
        }

        public override void Tick()
        {
            worker.surfaceOperations.Build(job);
        }

        override public void CancelJob()
        {
            job.Cancel();
        }

        override public void OnStateEnter()
        {
            job = (WorkerJob)worker.job;
            worker.SetIdleAnimation();
        }

        override public void OnStateExit()
        {

        }
    }
}