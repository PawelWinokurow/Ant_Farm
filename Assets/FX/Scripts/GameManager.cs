using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Surface Surface;
    public SurfaceOperations SurfaceOperations;
    public BuildTestMap BuildWallsTest;
    public JobScheduler JobScheduler;
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
        JobScheduler.Pathfinder = pathfinder;
        JobScheduler.SetSurfaceOperations(SurfaceOperations);
        JobScheduler.StartJobScheuler();
    }

    private void GenerateData()
    {
        PathGraph = new Graph();
        Surface.Init(PathGraph);
        BuildWallsTest.Init(Surface);
        pathfinder = new Pathfinder(PathGraph);
        JobScheduler.Pathfinder = pathfinder;
        JobScheduler.SetSurfaceOperations(SurfaceOperations);
        JobScheduler.StartJobScheuler();
        // StoreService.SaveGraph(PathGraph);
        // StoreService.SaveHexagons(Surface.Hexagons);
    }


    public void ProcessTap(Vector3 pos)
    {
        FloorHexagon hex = Surface.PositionToHex(pos);

        if (hex.Type == HEX_TYPE.FOOD)
        {
            var asssignedCarriersNum = ((CollectingHexagon)(hex.Child)).Carriers.Count;
            if (asssignedCarriersNum <= 3)
            {
                JobScheduler.AssignJob(new CarrierJob($"{hex.Id}_{asssignedCarriersNum + 1}", hex, Surface.BaseHex.Position, hex.Position));
            }
        }
        else if (hex.Type == HEX_TYPE.EMPTY || hex.Type == HEX_TYPE.SOIL)
        {
            if (AreNoMobsInHex(hex))
            {
                if (SurfaceOperations.IsInOldHexagons(hex))
                {
                    Surface.RemoveIcon(hex);
                    JobScheduler.CancelJob(hex.Id);
                }
                else
                {
                    Surface.AddIcon(hex);
                    if (hex.Type == HEX_TYPE.EMPTY)
                    {
                        JobScheduler.AssignJob(new WorkerJob(hex, hex.transform.position, JobType.FILL));
                    }
                    else if (hex.Type == HEX_TYPE.SOIL)
                    {
                        JobScheduler.AssignJob(new WorkerJob(hex, hex.transform.position, JobType.DIG));
                    }

                }
            }
        }
    }

    private bool AreNoMobsInHex(Hexagon hex)
    {
        return JobScheduler.AllMobs.All(mob => Surface.PositionToHex(mob.Position).Id != hex.Id);
    }
}

