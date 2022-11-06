using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodHexagon : MonoBehaviour, Hexagon
{
    public string Id { get => FloorHexagon.Id; set => FloorHexagon.Id = value; }
    public Vector3 Position { get => FloorHexagon.Position; set => FloorHexagon.Position = value; }
    public HEX_TYPE Type { get => FloorHexagon.Type; set => FloorHexagon.Type = value; }
    public int Food { get; set; }
    public static int MaxFood = 1000;
    public FloorHexagon FloorHexagon { get; set; }
    public List<Worker> Carriers { get; set; }

    public static FoodHexagon CreateHexagon(FloorHexagon parent, FoodHexagon workHexPrefab)
    {
        FoodHexagon hex = Instantiate(workHexPrefab, parent.Position, Quaternion.identity, parent.transform);
        hex.FloorHexagon = parent;
        hex.FloorHexagon.Child = hex;
        hex.Food = MaxFood;
        hex.Carriers = new List<Worker>();
        return hex;
    }

    public FoodHexagon AssignProperties(FoodHexagon hex)
    {
        Food = hex.Food;
        return this;
    }

    public void AssignWorker(Worker worker)
    {
        Carriers.Add(worker);
    }

}

