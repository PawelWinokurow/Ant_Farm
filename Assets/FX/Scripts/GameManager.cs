using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Surface Surface;
    public SurfaceOperations SurfaceOperations;
    public BuildTestMap BuildWallsTest;
    public WorkerJobScheduler WorkerJobScheduler;
    public EnemyJobScheduler EnemyJobScheduler;
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
        WorkerJobScheduler.Pathfinder = pathfinder;
        WorkerJobScheduler.Pathfinder = pathfinder;
        WorkerJobScheduler.SetSurfaceOperations(SurfaceOperations);
        WorkerJobScheduler.StartJobScheuler();
        EnemyJobScheduler.StartJobScheuler();
    }

    private void GenerateData()
    {
        PathGraph = new Graph();
        Surface.Init(PathGraph);
        BuildWallsTest.Init(Surface);
        pathfinder = new Pathfinder(PathGraph);
        EnemyJobScheduler.Pathfinder = pathfinder;
        WorkerJobScheduler.Pathfinder = pathfinder;
        WorkerJobScheduler.SetSurfaceOperations(SurfaceOperations);
        WorkerJobScheduler.StartJobScheuler();
        EnemyJobScheduler.StartJobScheuler();
        // StoreService.SaveGraph(PathGraph);
        // StoreService.SaveHexagons(Surface.Hexagons);
    }


    public void ProcessTap(Vector3 pos)
    {
        FloorHexagon hex = Surface.PositionToHex(pos);

        if (hex.Type == HEX_TYPE.FOOD)
        {
            var foodHex = (CollectingHexagon)(hex.Child);
            var asssignedCarriersNum = foodHex.Carriers.Count;
            if (asssignedCarriersNum < 5)
            {
                WorkerJobScheduler.AssignJob(new CarrierJob($"{hex.Id}_{asssignedCarriersNum + 1}", hex, (BaseHexagon)Surface.BaseHex.Child, foodHex));
                foodHex.Food.antCount++;
            }
            else
            {
                foodHex.ResetWorkers();
                foodHex.Food.antCount = 0;
            }
        }
        else if (hex.Type == HEX_TYPE.EMPTY || hex.Type == HEX_TYPE.SOIL)
        {
            if (AreNoMobsInHex(hex))
            {
                if (SurfaceOperations.IsInOldHexagons(hex) && !WorkerJobScheduler.IsJobAssigned(hex.Id))
                {
                    Surface.RemoveIcon(hex);
                    WorkerJobScheduler.CancelJob(hex.Id);
                }
                else
                {
                    Surface.AddIcon(hex);
                    if (hex.Type == HEX_TYPE.EMPTY)
                    {
                        WorkerJobScheduler.AssignJob(new WorkerJob(hex, hex.transform.position, JobType.FILL));
                    }
                    else if (hex.Type == HEX_TYPE.SOIL)
                    {
                        WorkerJobScheduler.AssignJob(new WorkerJob(hex, hex.transform.position, JobType.DIG));
                    }

                }
            }
        }
    }

    private bool AreNoMobsInHex(Hexagon hex)
    {
        return WorkerJobScheduler.AllMobs.All(mob => Surface.PositionToHex(mob.Position).Id != hex.Id);
    }
}

