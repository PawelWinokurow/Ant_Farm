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
    public Store store;
    public Pricing pricing;
    private ResourceOperations resourceOperations;

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
        workerJobScheduler.SetSurfaceOperations(surfaceOperations);
        workerJobScheduler.StartJobScheuler();

    }

    private void GenerateData()
    {
        pathGraph = new Graph();
        surface.Init(pathGraph);
        buildWallsTest.Init();
        pathfinder = new Pathfinder(pathGraph);
        workerJobScheduler.pathfinder = pathfinder;
        workerJobScheduler.SetSurfaceOperations(surfaceOperations);
        workerJobScheduler.StartJobScheuler();
        surface = Surface.Instance;
        surfaceOperations = SurfaceOperations.Instance;
        resourceOperations = ResourceOperations.Instance;
        store = Store.Instance;
        // StoreService.SaveGraph(pathGraph);
        // StoreService.SaveHexagons(Surface.Hexagons);
    }


    public void ProcessTap(Vector3 pos)
    {
        FloorHexagon hex = surface.PositionToHex(pos);

        if (surface.IsInOldHexagons(hex) && !workerJobScheduler.IsJobAssigned(hex.id))
        {
            resourceOperations.Sell(hex.position, pricing.pricesByHexType[surface.GetHexTypeByIcon(hex)]);
            workerJobScheduler.CancelJob(hex.id);
            surface.RemoveIcon(hex);
        }
        else
        {
            if ((slider.choosenValue == SliderValue.WORKER
            || slider.choosenValue == SliderValue.GOBBER
            || slider.choosenValue == SliderValue.GUNNER
            || slider.choosenValue == SliderValue.SCORPION
            || slider.choosenValue == SliderValue.SOLDIER
            || slider.choosenValue == SliderValue.ZOMBIE
            || slider.choosenValue == SliderValue.BLOB)
             && (hex.type == HexType.EMPTY || hex.type == HexType.SPIKES)
             && store.food >= pricing.pricesBySliderValue[slider.choosenValue])
            {
                mobFactory.AddMobByType(slider.choosenValue, hex);
                resourceOperations.Buy(hex.position, pricing.pricesBySliderValue[slider.choosenValue]);
            }
            else if ((slider.choosenValue == SliderValue.SOIL
            || slider.choosenValue == SliderValue.STONE
            || slider.choosenValue == SliderValue.SPIKES
            || slider.choosenValue == SliderValue.TURRET)
            && store.food >= pricing.pricesBySliderValue[slider.choosenValue])
            {
                if (hex.type == HexType.EMPTY)
                {
                    surface.PlaceIcon(hex, slider.choosenValue);
                    workerJobScheduler.AssignBuildJob(new BuildJob(hex, hex.transform.position, JobType.MOUNT));
                    resourceOperations.Buy(hex.position, pricing.pricesBySliderValue[slider.choosenValue]);
                }
                else if (slider.choosenValue == SliderValue.DEMOUNT)
                {
                    surface.PlaceIcon(hex, slider.choosenValue);
                    workerJobScheduler.AssignBuildJob(new BuildJob(hex, hex.transform.position, JobType.DEMOUNT));
                }
            }
        }
    }

    private bool AreNoMobsInHex(Hexagon hex)
    {
        return workerJobScheduler.allWorkers.Where(mob => mob.currentState.type != STATE.DEAD).All(mob => surface.PositionToHex(mob.position).id != hex.id);
    }
}

