using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Surface Surface;
    public BuildWallsTest BuildWallsTest;
    public JobScheduler JobScheduler;

    public Digger DiggerPrefab;

    public Graph PathGraph;

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
        JobScheduler.SetGraph(PathGraph);
        JobScheduler.SetSurface(Surface);
    }

    private void GenerateData()
    {
        PathGraph = new Graph();
        Surface.Init(PathGraph);
        BuildWallsTest.Init(Surface);
        JobScheduler.SetGraph(PathGraph);
        JobScheduler.SetSurface(Surface);
        StoreService.SaveGraph(PathGraph);
        StoreService.SaveHexagons(Surface.Hexagons);
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

