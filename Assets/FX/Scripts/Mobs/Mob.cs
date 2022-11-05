using UnityEngine;
using System;

public interface Mob
{
    public Vector3 CurrentPosition { get; }
    public bool HasJob { get; }
    public State CurrentState { get; set; }
    public Job Job { get; set; }
    public Path Path { get; set; }
    public bool HasPath { get; }
    public Pathfinder Pathfinder { get; set; }
    public void SetState(State state);
    public void SetPath(Path path);

}
