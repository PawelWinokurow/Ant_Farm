using System;
using UnityEngine;
public class DigJob : Job
{
    public string Id { set; get; }
    public bool IsAssigned { get; set; }
    public Vector3 Destination { set; get; }
    public Hexagon Hex { get; set; }
    public Assignment Assignment { get; set; }
    public Action RemoveJob { get; set; }
    public DigJob(Hexagon hex, Vector3 destination, Assignment assignment)
    {
        Id = hex.Id;
        Hex = hex;
        Destination = destination;
        Assignment = assignment;
        IsAssigned = false;
    }
}