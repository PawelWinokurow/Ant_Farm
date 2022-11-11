using System;
using UnityEngine;
public class WorkerJob : Job
{
    public string Id { get; set; }
    public Vector3 Destination { get; set; }
    public FloorHexagon Hex { get; set; }
    public Action Cancel { get; set; }
    public Action Execute { get; set; }
    public Mob Mob { get; set; }
    public JobType Type { get; set; }
    public Path Path { get; set; }


    public WorkerJob(FloorHexagon hex, Vector3 destination, JobType type)
    {
        Id = hex.Id;
        Hex = hex;
        Destination = destination;
        Type = type;
    }
}