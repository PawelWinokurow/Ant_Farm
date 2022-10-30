using System;
using UnityEngine;

public interface Job
{
    int Id { get; set; }
    Vector3 Destination { get; set; }
    public Action<Mob> Assignment { get; set; }
}