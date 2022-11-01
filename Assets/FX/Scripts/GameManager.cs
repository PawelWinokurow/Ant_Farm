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
        InstantiateTestMobs();
    }

    private void LoadData()
    {
        PathGraph = StoreService.LoadGraph();
        HexagonSerializable[] hexagons = StoreService.LoadHexagons();
        Surface.Init(PathGraph, hexagons);
        JobScheduler.SetGraph(PathGraph);
    }

    private void GenerateData()
    {
        PathGraph = new Graph();
        Surface.Init(PathGraph);
        BuildWallsTest.Init(Surface);
        JobScheduler.SetGraph(PathGraph);
        StoreService.SaveGraph(PathGraph);
        StoreService.SaveHexagons(Surface.Hexagons);
    }

    void InstantiateTestMobs()
    {
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(30, 0, 30), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(50, 0, 0), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(0, 0, 50), Quaternion.identity));
    }

    public void ProcessTap(Vector3 pos)
    {
        Hexagon hex = Surface.PositionToHex(pos);

        Surface.AddIcon(hex);

        if (hex.IsEmpty)
        {
            JobScheduler.AssignJob(new DigJob(hex, hex.transform.position, AssignmentFactory.CreateFillAssignment(hex)));
        }
        else if (hex.IsSoil)
        {
            JobScheduler.AssignJob(new DigJob(hex, hex.transform.position, AssignmentFactory.CreateDigAssignment(hex)));
        }
    }



}

