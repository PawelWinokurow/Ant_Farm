using System.Collections.Generic;
using System.Collections;
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
            collectingHex.type = HexType.EMPTY;
            collectingHex.floorHexagon.RemoveChildren();
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
        var toStore = Mathf.Min((workerSettings.LOADING_SPEED * Time.deltaTime), worker.carryingWeight);
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
            // StartCoroutine(Demount(job));
        }
        else if (job.type == JobType.MOUNT)
        {
            StartCoroutine(Mount(job));
        }
    }

    // public IEnumerator Demount(WorkerJob workerJob)
    // {
    //     var worker = workerJob.worker;
    //     var floorHex = workerJob.hex;
    //     var soilHex = (WorkHexagon)(workerJob.hex.child);
    //     floorHex.RemoveChildren();
    //     var scaledBlock = surface.AddDigScaledBlock(floorHex);
    //     while (scaledBlock != null)
    //     {
    //         scaledBlock.work -= workerSettings.CONSTRUCTION_SPEED;
    //         scaledBlock.transform.localScale = Vector3.one * scaledBlock.work / WorkHexagon.MAX_WORK;
    //         if (scaledBlock.work <= 0)
    //         {
    //             surface.AddGround(floorHex);
    //             surface.pathGraph.SetAccesabillity(floorHex, gameSettings.ACCESS_MASK_FLOOR);
    //             worker.job.Cancel();
    //             oldHexagons.Remove(floorHex.id);
    //             yield break;
    //         }
    //         yield return new WaitForSeconds(0.1f);
    //     }
    // }

    public IEnumerator Mount(WorkerJob workerJob)
    {
        var worker = workerJob.worker;
        var floorHex = workerJob.hex;

        var type = GetHexTypeByIcon(floorHex);
        floorHex.RemoveChildren();
        var scaledBlock = surface.AddMountScaledBlock(floorHex, type);
        while (scaledBlock != null)
        {
            scaledBlock.work -= workerSettings.CONSTRUCTION_SPEED;
            scaledBlock.transform.localScale = Vector3.one * (1 - scaledBlock.work / WorkHexagon.MAX_WORK);
            if (scaledBlock.work <= 0)
            {
                floorHex.RemoveChildren();
                surface.AddBlock(floorHex, type);
                surface.pathGraph.SetAccesabillity(floorHex, GetAccessMaskByHexType(type));
                worker.job.Cancel();
                oldHexagons.Remove(floorHex.id);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        surface.pathGraph.SetAccesabillity(floorHex, gameSettings.ACCESS_MASK_FLOOR);
        worker.job.Cancel();
    }

    private int GetAccessMaskByHexType(HexType type)
    {
        if (type == HexType.SOIL) return gameSettings.ACCESS_MASK_SOIL;
        else if (type == HexType.STONE) return gameSettings.ACCESS_MASK_STONE;
        else if (type == HexType.SPIKES) return gameSettings.ACCESS_MASK_SPIKES;
        return gameSettings.ACCESS_MASK_FLOOR;
    }
    private HexType GetHexTypeByIcon(FloorHexagon floorHexagon)
    {
        var tag = ((WorkHexagon)floorHexagon.child).tag;
        Debug.Log(tag);
        if (tag == "Soil_Mount_Icon") return HexType.SOIL;
        else if (tag == "Stone_Mount_Icon") return HexType.STONE;
        else if (tag == "Spikes_Mount_Icon") return HexType.SPIKES;
        return HexType.EMPTY;
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