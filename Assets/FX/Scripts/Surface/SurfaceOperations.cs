using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using WorkerNamespace;
using System;

public class SurfaceOperations : MonoBehaviour
{
    private Store store;
    private Surface surface;
    private GameSettings gameSettings;
    private WorkerSettings workerSettings;
    public static SurfaceOperations Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        workerSettings = Settings.Instance.workerSettings;
        gameSettings = Settings.Instance.gameSettings;
        surface = Surface.Instance;
        store = Store.Instance;
    }

    public void Loading(CollectingHexagon collectingHex, LoadingState state, CarrierJob job)
    {
        var worker = state.worker;
        var workerMaxCanTake = workerSettings.MAX_CARRYING_WEIGHT - worker.carryingWeight;
        var canTake = Mathf.Min((workerSettings.LOADING_SPEED * Time.deltaTime), workerMaxCanTake);
        if (canTake >= collectingHex.amount)
        {
            worker.carryingWeight += collectingHex.amount;
            collectingHex.amount = 0;
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
            collectingHex.amount -= toTake;
            if (worker.carryingWeight >= workerSettings.MAX_CARRYING_WEIGHT)
            {
                worker.carryingWeight = workerSettings.MAX_CARRYING_WEIGHT;
                state.Done();
            }
        }
    }

    public void Unloading(WorkHexagon storageHex, UnloadingState state, CarrierJob job)
    {
        var worker = state.worker;
        var toStore = worker.carryingWeight;
        store.AddRemoveFood(toStore);
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
                surface.RemoveBuilding(floorHex);
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
            workerJob.Cancel();
            surface.RemoveBuilding(floorHex);
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
        scaledBlock.transform.localScale = Vector3.one * Mathf.Lerp(icon.scl.x, 1, (1 - scaledBlock.work / WorkHexagon.MAX_WORK));
        if (scaledBlock.work <= 0)
        {
            workerJob.Cancel();
            surface.ClearHex(floorHex);
            var block = surface.AddBlock(floorHex, type);
            block.surface = surface;
            surface.pathGraph.SetAccesabillity(floorHex, surface.GetAccessMaskByHexType(type), surface.GetEdgeWeightByHexType(type));
        }
    }



    public WorkHexagon NearestBaseHexagon()
    {
        return (WorkHexagon)surface.baseHex.child;
    }

}