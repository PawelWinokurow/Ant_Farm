using System;
using UnityEngine;

public enum JobType
{
    DIG, FILL
}

public interface Job
{
    public string Id { get; set; }
    public Vector3 Destination { get; set; }
    public Mob Mob { get; set; }
    public JobType Type { get; set; }
    public Action Remove { get; set; }
    public Action Execute { get; set; }
}