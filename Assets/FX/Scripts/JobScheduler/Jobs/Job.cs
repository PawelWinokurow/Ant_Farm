using System;
using UnityEngine;

public enum JobType
{
    DIG, FILL, CARRYING, ATTACK
}

public interface Job
{
    public string id { get; set; }
    public Vector3 destination { get; set; }
    public FloorHexagon hex { get; set; }
    public Mob mob { get; set; }
    public JobType type { get; set; }
    public Action Cancel { get; set; }
    public Path path { get; set; }
}