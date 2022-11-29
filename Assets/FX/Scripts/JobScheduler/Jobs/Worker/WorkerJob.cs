using System;
using UnityEngine;
using WorkerNamespace;

public enum JobType
{
    NONE, DEMOUNT, SOIL, STONE, SPIKES, CARRYING
}

public interface WorkerJob
{
    public string id { get; set; }
    public Vector3 destination { get; set; }
    public FloorHexagon hex { get; set; }
    public JobType type { get; set; }
    public Worker worker { get; set; }
    public Action Cancel { get; set; }
    public Path path { get; set; }
}