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

        if (surfaceOperations.IsInOldHexagons(hex) && !workerJobScheduler.IsJobAssigned(hex.id))
        {
            surface.RemoveIcon(hex);
            workerJobScheduler.CancelJob(hex.id);
        }
        else
        {
            if (hex.type == HexType.EMPTY)
            {
                if (slider.choosenValue == SliderValue.WORKER)
                {
                    mobFactory.AddWorker(hex);
                }
                else if (slider.choosenValue == SliderValue.SOLDIER)
                {
                    mobFactory.AddSoldier(hex);
                }
                else if (slider.choosenValue == SliderValue.GUNNER)
                {
                    mobFactory.AddGunner(hex);
                }
                else if (slider.choosenValue == SliderValue.SCORPION)
                {
                    //Test only
                    mobFactory.AddScorpion(hex);
                }
                else if (slider.choosenValue == SliderValue.GOBBER)
                {
                    //Test only
                    mobFactory.AddGobber(hex);
                }
                else if (slider.choosenValue == SliderValue.SOIL
                || slider.choosenValue == SliderValue.STONE
                || slider.choosenValue == SliderValue.SPIKES)
                {
                    surface.PlaceIcon(hex, slider.choosenValue);
                    workerJobScheduler.AssignJob(new BuildJob(hex, hex.transform.position, JobType.MOUNT));
                }
            }
            else if ((hex.type == HexType.SOIL || hex.type == HexType.STONE || hex.type == HexType.SPIKES) && slider.choosenValue == SliderValue.DEMOUNT)
            {
                surface.PlaceIcon(hex, slider.choosenValue);
                workerJobScheduler.AssignJob(new BuildJob(hex, hex.transform.position, JobType.DEMOUNT));
            }
        }
    }

    private bool AreNoMobsInHex(Hexagon hex)
    {
        return workerJobScheduler.allWorkers.Where(mob => mob.currentState.type != STATE.DEAD).All(mob => surface.PositionToHex(mob.position).id != hex.id);
    }
}

