using System;
using UnityEngine;
public class DiggerJob : Job
{
    public string Id { set; get; }
    public Vector3 Destination { set; get; }
    public Hexagon Hex { get; set; }
    public Action Remove { get; set; }
    public Action Execute { get; set; }
    public Mob Mob { get; set; }
    public JobType Type { get; set; }

    public DiggerJob(Hexagon hex, Vector3 destination, JobType type)
    {
        Id = hex.Id;
        Hex = hex;
        Destination = destination;
        Type = type;
    }
}