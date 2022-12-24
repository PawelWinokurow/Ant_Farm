using UnityEngine;
using System;
using MobNamespace;


public enum MobType
{
    WORKER, SOLDIER, GUNNER, SCORPION, GOBBER, ZOMBIE, BLOB
}

public interface Mob
{
    public string id { get; set; }
    public MobType type { get; set; }
    public Vector3 position { get; }
    public State currentState { get; set; }
    public Path path { get; set; }
    public bool HasPath { get; }
    public Pathfinder pathfinder { get; set; }
    public FloorHexagon currentHex { get; set; }
    public Action Kill { get; set; }
    public Health health { get; set; }
    public int accessMask { get; set; }
    public void SetState(State state);
    public void SetPath(Path path);
    public void Move();
    public void RemovePath();
    public float Hit(int damage);
    public void SetRunAnimation();
    public void SetIdleAnimation();
}

