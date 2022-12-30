namespace WorkerNamespace
{

    public class GoToState : State
    {

        private Worker worker;
        private WorkerJob job;
        private WorkerSettings workerSettings = Settings.Instance.workerSettings;


        public GoToState(Worker worker)
        {
            this.type = STATE.GOTO;
            this.worker = worker;
        }

        override public void OnStateEnter()
        {
            worker.movementSpeed = workerSettings.MOVEMENT_SPEED;
            job = worker.job;
        }
        override public void OnStateExit()
        {
            worker.RemovePath();
        }

        override public void CancelJob()
        {
            if (worker.carryingWeight == 0)
            {
                job.Cancel();
            }
        }

        public override void Tick()
        {
            if (worker.HasPath)
            {
                if (!worker.currentPathEdge.HasAccess(worker.accessMask))
                {
                    worker.Rerouting();
                }
                else
                {
                    worker.Move();
                }
            }
            else
            {
                if (worker.job.type == JobType.MOUNT || worker.job.type == JobType.DEMOUNT)
                {
                    worker.SetState(new BuildState(worker));
                }
                else if (worker.job.type == JobType.CARRYING)
                {
                    var job = (CarrierJob)(worker.job);
                    if (job.direction == Direction.COLLECTING)
                    {
                        worker.SetState(new LoadingState(worker));
                    }
                    else if (job.direction == Direction.STORAGE)
                    {
                        worker.SetState(new UnloadingState(worker));

                    }
                }
            }
        }

    }
}