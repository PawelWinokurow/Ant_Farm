using System;
using UnityEngine;


public interface Job
{
    public string Id { get; set; }
    public bool IsAssigned { get; set; }
    public Vector3 Destination { get; set; }
    public Assignment Assignment { get; set; }
    public Action RemoveJob { get; set; }
}