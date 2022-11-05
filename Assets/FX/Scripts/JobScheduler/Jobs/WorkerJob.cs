using System;
using UnityEngine;
public class WorkerJob : Job
{
    public string Id { set; get; }
    public Vector3 Destination { set; get; }
    public FloorHexagon Hex { get; set; }
    public Action CancelJob { get; set; }
    public Action CancelNotCompleteJob { get; set; }
    public Action Execute { get; set; }
    public Action Return { get; set; }
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