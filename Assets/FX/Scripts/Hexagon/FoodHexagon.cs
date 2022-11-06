using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodHexagon : MonoBehaviour, Hexagon
{
    public string Id { get; set; }
    public HEX_TYPE Type { get => FloorHexagon.Type; set => FloorHexagon.Type = value; }
    public Vector3 Position { get; set; }
    public int Food { get; set; }
    public FloorHexagon FloorHexagon { get; set; }
    public List<Worker> Carriers { get; set; }

    public static FoodHexagon CreateHexagon(FloorHexagon parent, FoodHexagon workHexPrefab)
    {
        FoodHexagon hex = Instantiate(workHexPrefab, parent.Position, Quaternion.identity, parent.transform);
        hex.FloorHexagon = parent;
        hex.Position = parent.Position;
        hex.Id = parent.Id;
        hex.Food = 1000;
        hex.FloorHexagon.Child = hex;
        hex.Carriers = new List<Worker>();
        return hex;
    }

    public FoodHexagon AssignProperties(FoodHexagon hex)
    {
        Id = hex.Id;
        Type = hex.Type;
        Position = hex.Position;
        Food = hex.Food;
        return this;
    }

    public void AssignWorker(Worker worker)
    {
        Carriers.Add(worker);
    }

}

