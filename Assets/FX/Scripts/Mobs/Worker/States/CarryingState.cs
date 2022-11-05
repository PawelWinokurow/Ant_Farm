using UnityEngine;

public class GoToState : State
{

    private Worker worker;
    private int MOVEMENT_SPEED = 5;


    public GoToState(Worker worker) : base(worker)
    {
        this.Type = STATE.GOTO;
        this.worker = worker;
    }

    override public void OnStateEnter()
    {
    }
    override public void OnStateExit()
    {
        worker.RemovePath();
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
            worker.SetState(new DigState(worker));
        }
    }

}