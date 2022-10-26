using System;
using UnityEngine;
public class DigJob : Job
{
    public int Id { set; get; }
    public Vector3 Destination { set; get; }

    // public DigJob(Hexagon hex, Action<Hexagon> RemoveHexagonFunc)
    // {
    //     Id = hex.id;
    //     TargetHex = hex;

    // }
}