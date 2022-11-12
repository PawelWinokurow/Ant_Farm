using UnityEngine;
using System;

public interface Mob
{
    public string Id { get; set; }
    public Vector3 Position { get; }
    public bool HasJob { get; }
    public State CurrentState { get; set; }
    public Job Job { get; set; }
    public Path Path { get; set; }
    public bool HasPath { get; }
    public Pathfinder Pathfinder { get; set; }
    public float Hp { get; set; }
    public Action KillMob { get; set; }
    public void SetState(State state);
    public void SetPath(Path path);
    public void Move(int speed);
    public void RemovePath();


}
