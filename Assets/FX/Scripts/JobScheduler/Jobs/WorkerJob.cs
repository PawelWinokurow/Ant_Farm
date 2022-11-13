using System;
using UnityEngine;
public class WorkerJob : Job
{
    public string id { get; set; }
    public Vector3 destination { get; set; }
    public FloorHexagon hex { get; set; }
    public Action Cancel { get; set; }
    public Mob mob { get; set; }
    public JobType type { get; set; }
    public Path path { get; set; }


    public WorkerJob(FloorHexagon hex, Vector3 destination, JobType type)
    {
        id = hex.id;
        this.hex = hex;
        this.destination = destination;
        this.type = type;
    }
}