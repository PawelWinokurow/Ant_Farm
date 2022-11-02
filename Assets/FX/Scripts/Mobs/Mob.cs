using UnityEngine;
using System.Collections.Generic;

public interface Mob
{
    public Vector3 CurrentPosition { get; set; }
    public bool HasJob { get; }
    public State CurrentState { get; set; }
    public Job Job { get; set; }
    public Path Path { get; set; }
    public bool HasPath { get; }
    public Vector3 InitialPosition { get; set; }
    public void SetState(State state);
    public void SetPath(Path path);

}
