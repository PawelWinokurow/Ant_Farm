using UnityEngine;
using System;

public interface Mob
{
    public string id { get; set; }
    public Vector3 position { get; }
    public bool HasJob { get; }
    public State currentState { get; set; }
    public Job job { get; set; }
    public Path path { get; set; }
    public bool HasPath { get; }
    public Pathfinder pathfinder { get; set; }
    public FloorHexagon currentHex { get; set; }
    public float hp { get; set; }
    public Action Kill { get; set; }
    public void SetState(State state);
    public void SetPath(Path path);
    public void Move(int speed);
    public void RemovePath();


}
