using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Surface Surface;
    public BuildWallsTest BuildWallsTest;
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
        JobScheduler.SetSurface(Surface);
        JobScheduler.StartJobScheuler();
    }

    private void GenerateData()
    {
        PathGraph = new Graph();
        Surface.Init(PathGraph);
        BuildWallsTest.Init(Surface);
        pathfinder = new Pathfinder(PathGraph);
        JobScheduler.Pathfinder = pathfinder;
        JobScheduler.SetSurface(Surface);
        JobScheduler.StartJobScheuler();
        // StoreService.SaveGraph(PathGraph);
        // StoreService.SaveHexagons(Surface.Hexagons);
    }


    public void ProcessTap(Vector3 pos)
    {
        FloorHexagon hex = Surface.PositionToHex(pos);
        if (AreNoMobsInHex(hex))
        {
            if (Surface.IsInOldHexagons(hex))
            {
                Surface.RemoveIcon(hex);
            }
            if (JobScheduler.IsJobAlreadyCreated(hex.Id))
            {
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
                else if (hex.Type == HEX_TYPE.FOOD)
                {
                    JobScheduler.AssignJob(new CarrierJob(hex, Surface.BaseHex.Position, hex.transform.position));
                }
            }
        }


    }

    private bool AreNoMobsInHex(Hexagon hex)
    {
        return JobScheduler.AllMobs.All(mob => Surface.PositionToHex(mob.CurrentPosition).Id != hex.Id);
    }
}

