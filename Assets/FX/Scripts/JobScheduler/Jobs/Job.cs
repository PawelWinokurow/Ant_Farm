using System;
using UnityEngine;

public enum JobType
{
    DIG, FILL, CARRYING
}

public interface Job
{
    public string Id { get; set; }
    public Vector3 Destination { get; set; }
    public FloorHexagon Hex { get; set; }
    public Mob Mob { get; set; }
    public JobType Type { get; set; }
    public Action CancelJob { get; set; }
    public Action CancelNotCompleteJob { get; set; }
    public Action Execute { get; set; }
    public Path Path { get; set; }
}