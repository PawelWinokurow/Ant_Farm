using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Surface Surface;
    public SurfaceOperations SurfaceOperations;
    public BuildTestMap BuildWallsTest;
    public WorkerJobScheduler WorkerJobScheduler;
    public Graph PathGraph;
    private Pathfinder pathfinder;

    void Awake()
    {
        if (StoreService.DoesGraphExist() && StoreService.DoHexagonsExist())
        {
            LoadData();
        }
        else
        {
            GenerateData();
        }
    }

    private void LoadData()
    {
        PathGraph = StoreService.LoadGraph();
        HexagonSerializable[] hexagons = StoreService.LoadHexagons();
        Surface.Init(PathGraph, hexagons);
        pathfinder = new Pathfinder(PathGraph);
        WorkerJobScheduler.pathfinder = pathfinder;
        WorkerJobScheduler.pathfinder = pathfinder;
        WorkerJobScheduler.SetSurfaceOperations(SurfaceOperations);
        WorkerJobScheduler.StartJobScheuler();
    }

    private void GenerateData()
    {
        PathGraph = new Graph();
        Surface.Init(PathGraph);
        BuildWallsTest.Init(Surface);
        pathfinder = new Pathfinder(PathGraph);
        WorkerJobScheduler.pathfinder = pathfinder;
        WorkerJobScheduler.SetSurfaceOperations(SurfaceOperations);
        WorkerJobScheduler.StartJobScheuler();
        // StoreService.SaveGraph(PathGraph);
        // StoreService.SaveHexagons(Surface.Hexagons);
    }


    public void ProcessTap(Vector3 pos)
    {
        FloorHexagon hex = Surface.PositionToHex(pos);

        if (hex.type == HEX_TYPE.FOOD)
        {
            var foodHex = (CollectingHexagon)(hex.child);
            var asssignedcarriersNum = foodHex.carriers.Count;
            if (asssignedcarriersNum < 5)
            {
                WorkerJobScheduler.AssignJob(new CarrierJob($"{hex.id}_{asssignedcarriersNum + 1}", hex, (BaseHexagon)Surface.baseHex.child, foodHex));
                foodHex.food.antCount++;
            }
            else
            {
                foodHex.ResetWorkers();
                foodHex.food.antCount = 0;
            }
        }
        else if (hex.type == HEX_TYPE.EMPTY || hex.type == HEX_TYPE.SOIL)
        {
            if (AreNoMobsInHex(hex))
            {
                if (SurfaceOperations.IsInoldHexagons(hex) && !WorkerJobScheduler.IsJobAssigned(hex.id))
                {
                    Surface.RemoveIcon(hex);
                    WorkerJobScheduler.CancelJob(hex.id);
                }
                else
                {
                    Surface.AddIcon(hex);
                    if (hex.type == HEX_TYPE.EMPTY)
                    {
                        WorkerJobScheduler.AssignJob(new BuildJob(hex, hex.transform.position, JobType.FILL));
                    }
                    else if (hex.type == HEX_TYPE.SOIL)
                    {
                        WorkerJobScheduler.AssignJob(new BuildJob(hex, hex.transform.position, JobType.DIG));
                    }

                }
            }
        }
    }

    private bool AreNoMobsInHex(Hexagon hex)
    {
        return WorkerJobScheduler.allWorkers.All(mob => Surface.PositionToHex(mob.position).id != hex.id);
    }
}

