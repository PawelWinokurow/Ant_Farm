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
        JobScheduler.SetPathfinder(pathfinder);
        JobScheduler.SetSurface(Surface);
        JobScheduler.StartJobScheuler();
    }

    private void GenerateData()
    {
        PathGraph = new Graph();
        Surface.Init(PathGraph);
        BuildWallsTest.Init(Surface);
        pathfinder = new Pathfinder(PathGraph);
        JobScheduler.SetPathfinder(pathfinder);
        JobScheduler.SetSurface(Surface);
        JobScheduler.StartJobScheuler();
        // StoreService.SaveGraph(PathGraph);
        // StoreService.SaveHexagons(Surface.Hexagons);
    }


    public void ProcessTap(Vector3 pos)
    {
        Hexagon hex = Surface.PositionToHex(pos);

        if (JobScheduler.IsJobAlreadyCreated(hex.Id))
        {
            Surface.RemoveIcon(hex);
            JobScheduler.CancelJob(hex.Id);
        }
        else
        {
            if (hex.IsEmpty)
            {
                PathGraph.ProhibitHexagon(hex.transform.position);
                JobScheduler.AssignJob(new DiggerJob(hex, hex.transform.position, JobType.FILL));
            }
            else if (hex.IsSoil)
            {
                JobScheduler.AssignJob(new DiggerJob(hex, hex.transform.position, JobType.DIG));
            }
            Surface.AddIcon(hex);
        }
    }



}

