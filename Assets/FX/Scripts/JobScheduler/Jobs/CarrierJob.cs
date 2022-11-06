using System;
using UnityEngine;
public class CarrierJob : Job
{
    public string Id { set; get; }
    public Vector3 Destination { set; get; }
    public Vector3 Departure { set; get; }
    public FloorHexagon Hex { get; set; }
    public Action CancelJob { get; set; }
    public Action ReturnToBase { get; set; }
    public Action Execute { get; set; }
    public Action CancelNotCompleteJob { get; set; }
    public Action Return { get; set; }
    public Mob Mob { get; set; }
    public JobType Type { get; set; }
    public Path Path { get; set; }

    public CarrierJob(FloorHexagon hex, Vector3 departure, Vector3 destination)
    {
        Id = hex.Id;
        Hex = hex;
        Departure = departure;
        Destination = destination;
        Type = JobType.CARRYING;
    }

}