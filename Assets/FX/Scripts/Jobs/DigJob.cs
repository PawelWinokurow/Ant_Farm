using System;
using UnityEngine;
public class DigJob : Job
{
    public int Id { set; get; }
    public Hexagon TargetHex { set; get; }
    public Vector3 Destination { set; get; }
    public Action<Hexagon> RemoveHexagonFunc { set; get; }
    public DigJob(Hexagon hex, Action<Hexagon> RemoveHexagonFunc)
    {
        Id = hex.id;
        TargetHex = hex;
        Destination = hex.transform.position;
        this.RemoveHexagonFunc = RemoveHexagonFunc;
    }
}