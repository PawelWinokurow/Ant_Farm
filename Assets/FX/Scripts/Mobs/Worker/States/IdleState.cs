using UnityEngine;

public class IdleState : State
{

    private Worker worker;
    private int MOVEMENT_SPEED = 5;

    public IdleState(Worker worker) : base(worker)
    {
        this.Type = STATE.IDLE;
        this.worker = worker;
    }
    public override void Tick()
    {
        if (worker.HasPath)
        {
            worker.AntAnimator.Run();
            worker.Move(MOVEMENT_SPEED);
        }
        else
        {
            worker.SetRandomWalk();
        }
    }

    override public void OnStateEnter()
    {
        worker.RemovePath();
    }
    override public void OnStateExit()
    {
    }
}