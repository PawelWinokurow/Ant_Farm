using System;
using UnityEngine;
public class DigJob : Job
{
    public int Id { set; get; }
    public Vector3 Destination { set; get; }

    public DigJob(Hexagon hex)
    {
        Id = hex.id;
    }
}