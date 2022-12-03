using System;
using UnityEngine;
using WorkerNamespace;

public enum Direction
{
    STORAGE, COLLECTING, CANCELED
}

public class CarrierJob : WorkerJob
{
    public string id { get; set; }
    public Vector3 destination { get; set; }
    public Direction direction { get; set; }
    public CollectingHexagon collectingHexagon { get; set; }
    public BaseHexagon storageHexagon { get; set; }
    public FloorHexagon hex { get; set; }
    public Action Cancel { get; set; }
    public Worker worker { get; set; }
    public JobType type { get; set; }
    public Path path { get; set; }

    public CarrierJob(FloorHexagon hex, BaseHexagon storageHex)
    {
        this.hex = hex;
        this.id = hex.id;
        direction = Direction.COLLECTING;
        storageHexagon = storageHex;
        collectingHexagon = (CollectingHexagon)hex.child;
        destination = collectingHexagon.position;
        type = JobType.CARRYING;
    }

    public void SwapDestination()
    {
        direction = direction == Direction.STORAGE ? Direction.COLLECTING : Direction.STORAGE;
        destination = destination == storageHexagon.position ? collectingHexagon.position : storageHexagon.position;
    }

}