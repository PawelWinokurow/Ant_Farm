public enum STATE
{
    IDLE, GOTO, BUILD, CARRYING, LOADING, UNLOADING, FOLLOWING, ATTACK, DEAD, PATROL, DIG
}

public abstract class State
{
    protected Mob mob;

    public STATE type { get; set; }
    public abstract void Tick();

    public abstract void CancelJob();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(Mob mob)
    {
        this.mob = mob;
    }
}