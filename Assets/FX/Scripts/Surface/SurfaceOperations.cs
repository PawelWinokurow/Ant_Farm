using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using WorkerNamespace;

public class SurfaceOperations : MonoBehaviour
{
    public Surface surface;
    public Dictionary<string, Hexagon> oldHexagons { get => surface.oldhexagons; set => surface.oldhexagons = value; }
    private GameSettings gameSettings;
    private WorkerSettings workerSettings;

    void Start()
    {
        workerSettings = Settings.Instance.workerSettings;
        gameSettings = Settings.Instance.gameSettings;
    }

    public void Loading(CollectingHexagon collectingHex, LoadingState state, CarrierJob job)
    {
        var worker = state.worker;
        var workerMaxCanTake = workerSettings.MAX_CARRYING_WEIGHT - worker.carryingWeight;
        var canTake = Mathf.Min((workerSettings.LOADING_SPEED * Time.deltaTime), workerMaxCanTake);
        if (canTake >= collectingHex.Quantity)
        {
            worker.carryingWeight += collectingHex.Quantity;
            collectingHex.Quantity = 0;
            collectingHex.carriers.Where(w => w.id != worker.id).ToList().ForEach(w => w.CancelJob());
            collectingHex.ResetWorkers();
            collectingHex.floorHexagon.RemoveChildren();
            collectingHex.type = HexType.EMPTY;
            state.Done();
        }
        else
        {
            var toTake = Mathf.Min(canTake, workerMaxCanTake);
            worker.carryingWeight += toTake;
            collectingHex.Quantity -= toTake;
            if (worker.carryingWeight >= workerSettings.MAX_CARRYING_WEIGHT)
            {
                state.Done();
            }
        }
    }

    public void Unloading(BaseHexagon storageHex, UnloadingState state, CarrierJob job)
    {
        var worker = state.worker;
        var toStore = Mathf.Min((workerSettings.UPLOADING_SPEED * Time.deltaTime), worker.carryingWeight);
        storageHex.storage += toStore;
        worker.carryingWeight -= toStore;
        if (worker.carryingWeight <= 0)
        {
            worker.carryingWeight = 0;
            state.Done();
        }
    }

    public void Build(WorkerJob job)
    {
        if (job.type == JobType.DEMOUNT)
        {
            Demount(job);
        }
        else if (job.type == JobType.MOUNT)
        {
            Mount(job);
        }
    }

    public void Dig(DigJob digJob)
    {
        var scorpion = digJob.scorpion;
        var floorHex = digJob.hex;
        if (floorHex.child == null || floorHex.child.Equals(null))
        {
            scorpion.CancelDigging();
        }
        else
        {
            var type = surface.GetHexTypeByIcon(floorHex);
            var scaledBlock = (WorkHexagon)(floorHex.child);
            scaledBlock.work -= scorpion.mobSettings.DIG_SPEED * Time.deltaTime;
            scaledBlock.transform.localScale = 0.95f * Vector3.one * scaledBlock.work / WorkHexagon.MAX_WORK;
            if (scaledBlock.work <= 0)
            {
                surface.AddGround(floorHex);
                surface.pathGraph.SetAccesabillity(floorHex, gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
                scorpion.CancelDigging();
            }
        }
    }

    public void Demount(WorkerJob workerJob)
    {
        var worker = workerJob.worker;
        var floorHex = workerJob.hex;
        var type = surface.GetHexTypeByIcon(floorHex);
        var scaledBlock = (WorkHexagon)(floorHex.child);

        scaledBlock.work -= worker.workerSettings.CONSTRUCTION_SPEED * Time.deltaTime;
        scaledBlock.transform.localScale = 0.95f * Vector3.one * scaledBlock.work / WorkHexagon.MAX_WORK;
        if (scaledBlock.work <= 0)
        {
            surface.AddGround(floorHex);
            surface.pathGraph.SetAccesabillity(floorHex, gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
            workerJob.Cancel();
            oldHexagons.Remove(floorHex.id);
        }
    }

    public void Mount(WorkerJob workerJob)
    {
        var worker = workerJob.worker;
        var floorHex = workerJob.hex;
        var type = surface.GetHexTypeByIcon(floorHex);
        var icon = ((WorkHexagon)floorHex.child).GetComponent<MountIcon>();
        var scaledBlock = icon.scaledIconPrefab;

        scaledBlock.work -= worker.workerSettings.CONSTRUCTION_SPEED * Time.deltaTime;
        scaledBlock.transform.localScale = Vector3.one * (0.4f + 0.6f * (1 - scaledBlock.work / WorkHexagon.MAX_WORK));
        if (scaledBlock.work <= 0)
        {
            floorHex.RemoveChildren();
            surface.AddBlock(floorHex, type);
            surface.pathGraph.SetAccesabillity(floorHex, surface.GetAccessMaskByHexType(type), surface.GetEdgeWeightByHexType(type));
            workerJob.Cancel();
            oldHexagons.Remove(floorHex.id);
        }
    }

    public bool IsInOldHexagons(FloorHexagon hex)
    {
        return oldHexagons.ContainsKey(hex.id);
    }

    public BaseHexagon NearestBaseHexagon()
    {
        return (BaseHexagon)surface.baseHex.child;
    }

}