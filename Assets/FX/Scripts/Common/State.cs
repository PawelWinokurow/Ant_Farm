
public enum STATE
{
    IDLE, GOTO, BUILD, CARRYING, LOADING, UNLOADING, FOLLOWING, ATTACK, DEAD, PATROL, DIG
}

public abstract class State
{
    public STATE type { get; set; }
    public abstract void Tick();

    public abstract void CancelJob();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

}
