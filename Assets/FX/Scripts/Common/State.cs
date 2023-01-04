public abstract class State
{
    public STATE type { get; set; }
    public abstract void Tick();

    public abstract void CancelJob();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

}
