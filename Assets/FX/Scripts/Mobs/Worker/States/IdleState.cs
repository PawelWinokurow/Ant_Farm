using UnityEngine;
namespace WorkerNamespace
{

    public class IdleState : State
    {

        private Worker worker;
        private WorkerSettings workerSettings = Settings.Instance.workerSettings;

        public IdleState(Worker worker) : base(worker)
        {
            this.type = STATE.IDLE;
            this.worker = worker;
            worker.path = null;
        }
        public override void Tick()
        {
            if (worker.HasPath)
            {
                worker.Move();
                if (worker.path.wayPoints.Count == 1)
                {
                    worker.ExpandRandomWalk();
                }
            }
            else
            {
                worker.SetRandomWalk();
            }
        }

        override public void OnStateEnter()
        {
            worker.movementSpeed = workerSettings.MOVEMENT_SPEED;
            worker.RemovePath();
            worker.SetRunAnimation();
            worker.Animation();
        }
        override public void OnStateExit()
        {
        }

        override public void CancelJob()
        {

        }
    }
}