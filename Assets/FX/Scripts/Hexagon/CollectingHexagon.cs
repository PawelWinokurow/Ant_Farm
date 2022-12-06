using System.Collections.Generic;
using UnityEngine;
using WorkerNamespace;

public class CollectingHexagon : MonoBehaviour, Hexagon
{
    public string id { get => floorHexagon.id; set => floorHexagon.id = value; }
    public Vector3 position { get => floorHexagon.position; set => floorHexagon.position = value; }
    public HexType type { get => floorHexagon.type; set => floorHexagon.type = value; }
    public float Quantity { get => food.cost; set { food.cost = value; } }

    public static float maxQuantity = 1000;
    public FloorHexagon floorHexagon { get; set; }
    public List<Worker> carriers { get; set; }
    private Food food { get; set; }

    public void Awake()
    {
        food = GetComponent<Food>();
    }

    public static CollectingHexagon CreateHexagon(FloorHexagon parent, CollectingHexagon workHexPrefab)
    {
        CollectingHexagon hex = Instantiate(workHexPrefab, parent.position, Quaternion.identity, parent.transform);
        hex.floorHexagon = parent;
        hex.floorHexagon.child = hex;
        hex.carriers = new List<Worker>();
        hex.food.costMax = maxQuantity;
        hex.food.cost = maxQuantity;
        return hex;
    }

    public CollectingHexagon AssignProperties(CollectingHexagon hex)
    {
        food.cost = hex.food.cost;
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

