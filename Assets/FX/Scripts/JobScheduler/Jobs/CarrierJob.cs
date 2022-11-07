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
    public Direction Direction { get; set; }
    public CollectingHexagon CollectingHexagon { get; set; }
    public BaseHexagon StorageHexagon { get; set; }
    public FloorHexagon Hex { get; set; }
    public Action Cancel { get; set; }
    public Action ReturnToBase { get; set; }
    public Action Execute { get; set; }
    public Action CancelNotCompleteJob { get; set; }
    public Action Return { get; set; }
    public Worker Worker { get; set; }
    public JobType Type { get; set; }
    public Path Path { get; set; }

    public CarrierJob(string id, FloorHexagon hex, BaseHexagon storageHex, CollectingHexagon collectingHex)
    {
        Id = id;
        Hex = hex;
        Destination = collectingHex.Position;
        Direction = Direction.COLLECTING;
        StorageHexagon = storageHex;
        CollectingHexagon = collectingHex;
        Type = JobType.CARRYING;
    }

    public void SwapDestination()
    {
        Direction = Direction == Direction.STORAGE ? Direction.COLLECTING : Direction.STORAGE;
        Destination = Destination == StorageHexagon.Position ? CollectingHexagon.Position : StorageHexagon.Position;
    }

}