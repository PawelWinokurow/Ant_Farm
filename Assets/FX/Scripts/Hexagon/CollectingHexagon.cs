using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectingHexagon : MonoBehaviour, Hexagon
{
    public string Id { get => FloorHexagon.Id; set => FloorHexagon.Id = value; }
    public Vector3 Position { get => FloorHexagon.Position; set => FloorHexagon.Position = value; }
    public HEX_TYPE Type { get => FloorHexagon.Type; set => FloorHexagon.Type = value; }
    private float quantity;
    public float Quantity { get => quantity; set { quantity = value; Food.cost = value; } }
    public static float MaxQuantity = 1000;
    public FloorHexagon FloorHexagon { get; set; }
    public List<Worker> Carriers { get; set; }
    public Food Food { get; set; }

    public void Awake()
    {
        Food = GetComponent<Food>();
    }

    public static CollectingHexagon CreateHexagon(FloorHexagon parent, CollectingHexagon workHexPrefab)
    {
        CollectingHexagon hex = Instantiate(workHexPrefab, parent.Position, Quaternion.identity, parent.transform);
        hex.FloorHexagon = parent;
        hex.FloorHexagon.Child = hex;
        hex.Quantity = MaxQuantity;
        hex.Carriers = new List<Worker>();
        return hex;
    }

    public CollectingHexagon AssignProperties(CollectingHexagon hex)
    {
        Quantity = hex.Quantity;
        return this;
    }

    public void AssignWorker(Worker worker)
    {
        Carriers.Add(worker);
    }

    public void ResetWorkers()
    {
        Carriers.ForEach(worker => worker.CancelJob());
        Carriers.Clear();
    }

}

