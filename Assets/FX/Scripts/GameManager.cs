using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Surface surface;
    public SurfaceOperations surfaceOperations;
    public BuildTestMap buildWallsTest;
    public WorkerJobScheduler workerJobScheduler;
    public Graph pathGraph;
    public ScrollRectSnap slider;
    public MobFactory mobFactory;
    private Pathfinder pathfinder;
    public GameSettings settings;

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
        pathGraph = StoreService.LoadGraph();
        HexagonSerializable[] hexagons = StoreService.LoadHexagons();
        surface.Init(pathGraph, hexagons);
        pathfinder = new Pathfinder(pathGraph);
        workerJobScheduler.pathfinder = pathfinder;
        workerJobScheduler.pathfinder = pathfinder;
        workerJobScheduler.SetSurfaceOperations(surfaceOperations);
        workerJobScheduler.StartJobScheuler();
    }

    private void GenerateData()
    {
        pathGraph = new Graph();
        surface.Init(pathGraph);
        buildWallsTest.Init(surface);
        pathfinder = new Pathfinder(pathGraph);
        workerJobScheduler.pathfinder = pathfinder;
        workerJobScheduler.SetSurfaceOperations(surfaceOperations);
        workerJobScheduler.StartJobScheuler();
        // StoreService.SaveGraph(pathGraph);
        // StoreService.SaveHexagons(Surface.Hexagons);
    }


    public void ProcessTap(Vector3 pos)
    {
        FloorHexagon hex = surface.PositionToHex(pos);


        if (slider.choosenValue == SliderValues.None)
        {

        }
        else if (slider.choosenValue == SliderValues.Soil)
        {
        }
        else if (slider.choosenValue == SliderValues.Stone)
        {

        }
        else if (slider.choosenValue == SliderValues.Spikes)
        {
        }
        else if (slider.choosenValue == SliderValues.Worker)
        {
            mobFactory.AddWorker();
        }
        else if (slider.choosenValue == SliderValues.Soldier)
        {
            mobFactory.AddSoldier();
        }



        // if (hex.type == HexType.FOOD)
        // {
        //     var foodHex = (CollectingHexagon)(hex.child);
        //     var asssignedcarriersNum = foodHex.carriers.Count;
        //     if (asssignedcarriersNum < 5)
        //     {
        //         workerJobScheduler.AssignJob(new CarrierJob($"{hex.id}_{asssignedcarriersNum + 1}", hex, (BaseHexagon)surface.baseHex.child, foodHex));
        //         foodHex.food.antCount++;
        //     }
        //     else
        //     {
        //         foodHex.ResetWorkers();
        //         foodHex.food.antCount = 0;
        //     }
        // }
        // else if (hex.type == HexType.EMPTY || hex.type == HexType.SOIL)
        // {
        //     if (AreNoMobsInHex(hex))
        //     {
        //         if (surfaceOperations.IsInOldHexagons(hex) && !workerJobScheduler.IsJobAssigned(hex.id))
        //         {
        //             surface.RemoveIcon(hex);
        //             workerJobScheduler.CancelJob(hex.id);
        //         }
        //         else
        //         {
        //             surface.AddIcon(hex);
        //             if (hex.type == HexType.EMPTY)
        //             {
        //                 workerJobScheduler.AssignJob(new BuildJob(hex, hex.transform.position, JobType.FILL));
        //             }
        //             else if (hex.type == HexType.SOIL)
        //             {
        //                 workerJobScheduler.AssignJob(new BuildJob(hex, hex.transform.position, JobType.DIG));
        //             }

        //         }
        //     }
        // }
    }



    private bool AreNoMobsInHex(Hexagon hex)
    {
        return workerJobScheduler.allWorkers.Where(mob => mob.currentState.type != STATE.DEAD).All(mob => surface.PositionToHex(mob.position).id != hex.id);
    }
}

