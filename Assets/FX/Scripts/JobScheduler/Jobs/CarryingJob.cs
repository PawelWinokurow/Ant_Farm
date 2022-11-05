using System;
using UnityEngine;
public class CarryingJob : Job
{
    public string Id { set; get; }
    public Vector3 Destination { set; get; }
    public Vector3 Departure { set; get; }
    public Hexagon Hex { get; set; }
    public Action CancelJob { get; set; }
    public Action Execute { get; set; }
    public Action CancelNotCompleteJob { get; set; }
    public Action Return { get; set; }
    public Mob Mob { get; set; }
    public JobType Type { get; set; }
    public Path Path { get; set; }


    public CarryingJob(Hexagon hex, Vector3 departure, Vector3 destination)
    {
        Id = hex.Id;
        Hex = hex;
        Departure = departure;
        Destination = destination;
        Type = JobType.CARRYING;
    }
}