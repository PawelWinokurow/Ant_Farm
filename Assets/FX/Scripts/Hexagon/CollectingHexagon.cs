using System.Collections.Generic;
using UnityEngine;
using WorkerNamespace;

public class CollectingHexagon : MonoBehaviour, Hexagon
{
    public string id { get => floorHexagon.id; set => floorHexagon.id = value; }
    public Vector3 position { get => floorHexagon.position; set => floorHexagon.position = value; }
    public HexType type { get => floorHexagon.type; set => floorHexagon.type = value; }
    public float amount { get => food.amount; set { food.amount = value; } }
    public static float maxAmount;
    public FloorHexagon floorHexagon { get; set; }
    public List<Worker> carriers { get; set; }
    public Food food { get; set; }

    public void Awake()
    {
        food = GetComponent<Food>();
    }

    public static CollectingHexagon CreateHexagon(FloorHexagon parent, CollectingHexagon workHexPrefab, int maxAmount)
    {
        CollectingHexagon hex = Instantiate(workHexPrefab, parent.position, Quaternion.identity, parent.transform);
        hex.floorHexagon = parent;
        hex.floorHexagon.child = hex;
        hex.food.amountMax = maxAmount;
        hex.food.amount = maxAmount;
        hex.carriers = new List<Worker>();
        return hex;
    }

    public CollectingHexagon AssignProperties(CollectingHexagon hex)
    {
        food.amount = hex.food.amount;
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

