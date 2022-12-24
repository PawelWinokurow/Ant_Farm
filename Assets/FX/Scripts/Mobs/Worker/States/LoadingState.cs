using MobNamespace;

namespace WorkerNamespace
{

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
            if (IsDone)
            {
                job.SwapDestination();
                worker.SetPath(worker.pathfinder.FindPath(worker.position, job.destination, worker.accessMask, SearchType.NEAREST_CENTRAL_VERTEX));
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
            if (worker.carryingWeight != 0)
            {
                worker.SetPath(worker.pathfinder.FindPath(worker.position, job.storageHexagon.position, worker.accessMask, SearchType.NEAREST_CENTRAL_VERTEX));
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
}