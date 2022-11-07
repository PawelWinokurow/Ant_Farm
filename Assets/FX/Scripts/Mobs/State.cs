public enum STATE
{
    IDLE, GOTO, DIG, CARRYING, LOADING, UNLOADING
}

public abstract class State
{
    protected Mob mob;

    public STATE Type { get; set; }
    public abstract void Tick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(Mob mob)
    {
        this.mob = mob;
    }
}