using System;
using UnityEngine;

public enum Direction
{
    STORAGE, COLLECTING, CANCELED
}

public class CarrierJob : Job
{
    public string id { get; set; }
    public Vector3 destination { get; set; }
    public Direction direction { get; set; }
    public CollectingHexagon collectingHexagon { get; set; }
    public BaseHexagon storageHexagon { get; set; }
    public FloorHexagon hex { get; set; }
    public Action Cancel { get; set; }
    public Mob mob { get; set; }
    public JobType type { get; set; }
    public Path path { get; set; }

    public CarrierJob(string id, FloorHexagon hex, BaseHexagon storageHex, CollectingHexagon collectingHex)
    {
        this.id = id;
        this.hex = hex;
        destination = collectingHex.position;
        direction = Direction.COLLECTING;
        storageHexagon = storageHex;
        collectingHexagon = collectingHex;
        type = JobType.CARRYING;
    }

    public void SwapDestination()
    {
        direction = direction == Direction.STORAGE ? Direction.COLLECTING : Direction.STORAGE;
        destination = destination == storageHexagon.position ? collectingHexagon.position : storageHexagon.position;
    }

}