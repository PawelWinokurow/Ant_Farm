
public class CarryingState : State
{
    private Worker worker;

    public CarryingState(Worker worker) : base(worker)
    {
        this.Type = STATE.CARRYING;
        this.worker = worker;
    }

    public override void Tick()
    {
        worker.AntAnimator.Idle();
    }

    override public void OnStateEnter()
    {
        worker.Job.Execute();
    }

    override public void OnStateExit()
    {

    }

}