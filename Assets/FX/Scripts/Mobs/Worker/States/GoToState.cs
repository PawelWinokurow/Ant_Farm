using UnityEngine;
namespace WorkerNamespace
{

    public class GoToState : State
    {

        private Worker worker;
        private WorkerJob job;
        private int MOVEMENT_SPEED = 5;


        public GoToState(Worker worker) : base(worker)
        {
            this.type = STATE.GOTO;
            this.worker = worker;
        }

        override public void OnStateEnter()
        {
            job = worker.job;
            worker.Animation();

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
                    worker.Move(MOVEMENT_SPEED);
                }
            }
            else
            {
                if (worker.job.type == JobType.DIG || worker.job.type == JobType.FILL)
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