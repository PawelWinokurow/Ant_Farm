public interface StateMachine
{
    public State currentState { get; set; }
    public void SetInitialState();
    public void SetState(State state);

}