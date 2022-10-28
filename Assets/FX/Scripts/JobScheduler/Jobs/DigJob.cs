using System;
using UnityEngine;
public class DigJob : Job
{
    public int Id { set; get; }
    public Vector3 Destination { set; get; }
    public Hexagon Hex { get; set; }

    public DigJob(Hexagon hex, Vector3 destination)
    {
        Id = hex.id;
        Hex = hex;
        Destination = destination;
    }
}