using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorkerNamespace;

public class CollectingHexagon : MonoBehaviour, Hexagon
{
    public string id { get => floorHexagon.id; set => floorHexagon.id = value; }
    public Vector3 position { get => floorHexagon.position; set => floorHexagon.position = value; }
    public HEX_TYPE type { get => floorHexagon.type; set => floorHexagon.type = value; }
    private float quantity;
    public float Quantity { get => quantity; set { quantity = value; food.cost = value; } }
    public static float maxQuantity = 1000;
    public FloorHexagon floorHexagon { get; set; }
    public List<Worker> carriers { get; set; }
    public Food food { get; set; }

    public void Awake()
    {
        food = GetComponent<Food>();
    }

    public static CollectingHexagon CreateHexagon(FloorHexagon parent, CollectingHexagon workHexPrefab)
    {
        CollectingHexagon hex = Instantiate(workHexPrefab, parent.position, Quaternion.identity, parent.transform);
        hex.floorHexagon = parent;
        hex.floorHexagon.child = hex;
        hex.Quantity = maxQuantity;
        hex.carriers = new List<Worker>();
        return hex;
    }

    public CollectingHexagon AssignProperties(CollectingHexagon hex)
    {
        Quantity = hex.Quantity;
        return this;
    }

    public void AssignWorker(Worker worker)
    {
        carriers.Add(worker);
    }

    public void ResetWorkers()
    {
        carriers.ForEach(worker => worker.CancelJob());
        carriers.Clear();
    }

}

