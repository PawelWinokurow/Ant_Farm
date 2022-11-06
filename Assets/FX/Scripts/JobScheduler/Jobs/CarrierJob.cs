using System;
using UnityEngine;

public enum Direction
{
    STORAGE, COLLECTING, CANCELED
}

public class CarrierJob : Job
{
    public string Id { get; set; }
    public Vector3 Destination { get; set; }
    public Vector3 CollectingPointPosition { get; set; }
    public Vector3 StoragePosition { get; set; }
    public Direction Direction { get; set; }
    public FloorHexagon Hex { get; set; }
    public Action CancelJob { get; set; }
    public Action ReturnToBase { get; set; }
    public Action Execute { get; set; }
    public Action CancelNotCompleteJob { get; set; }
    public Action Return { get; set; }
    public Worker Worker { get; set; }
    public JobType Type { get; set; }
    public Path Path { get; set; }

    public CarrierJob(string id, FloorHexagon hex, Vector3 storagePosition, Vector3 collectingPosition)
    {
        Id = id;
        Hex = hex;
        StoragePosition = storagePosition;
        CollectingPointPosition = collectingPosition;
        Destination = collectingPosition;
        Type = JobType.CARRYING;
    }

}