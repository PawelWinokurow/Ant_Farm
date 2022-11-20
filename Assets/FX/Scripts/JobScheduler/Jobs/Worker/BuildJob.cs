using System;
using UnityEngine;
using WorkerNamespace;
public class BuildJob : WorkerJob
{
    public string id { get; set; }
    public Vector3 destination { get; set; }
    public FloorHexagon hex { get; set; }
    public Action Cancel { get; set; }
    public Worker worker { get; set; }
    public JobType type { get; set; }
    public Path path { get; set; }


    public BuildJob(FloorHexagon hex, Vector3 destination, JobType type)
    {
        id = hex.id;
        this.hex = hex;
        this.destination = destination;
        this.type = type;
    }
}