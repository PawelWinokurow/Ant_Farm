using System;
using UnityEngine;

public enum JobType
{
    DIG, FILL, CARRYING, ATTACK
}

public interface Job
{
    public string Id { get; set; }
    public Vector3 Destination { get; set; }
    public FloorHexagon Hex { get; set; }
    public Mob Mob { get; set; }
    public JobType Type { get; set; }
    public Action Cancel { get; set; }
    public Path Path { get; set; }
}