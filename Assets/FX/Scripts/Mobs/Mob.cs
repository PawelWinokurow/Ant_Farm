using UnityEngine;
using System.Collections.Generic;

public interface Mob
{
    public Vector3 CurrentPosition { get; set; }
    public List<Vector3> WayPoints { get; set; }
    public bool HasJob { get; }
    public State CurrentState { get; set; }
    public Job Job { get; set; }
    public Path Path { get; set; }
    public void SetState(State state);
}
