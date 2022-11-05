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
        Hexagon hex = Surface.PositionToHex(pos);
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
                if (hex.IsEmpty)
                {
                    JobScheduler.AssignJob(new WorkerJob(hex, hex.transform.position, JobType.FILL));
                }
                else if (hex.IsSoil)
                {
                    JobScheduler.AssignJob(new WorkerJob(hex, hex.transform.position, JobType.DIG));
                }
                else if (hex.IsFood)
                {
                    JobScheduler.AssignJob(new CarryingJob(hex, hex.transform.position, Surface.BaseHex.Position));
                }
            }
        }


    }

    private bool AreNoMobsInHex(Hexagon hex)
    {
        return JobScheduler.AllMobs.All(mob => Surface.PositionToHex(mob.CurrentPosition).Id != hex.Id);
    }
}

