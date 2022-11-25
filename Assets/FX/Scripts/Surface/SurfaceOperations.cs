using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using WorkerNamespace;

public class SurfaceOperations : MonoBehaviour
{
    public Surface surface;
    public Dictionary<string, Hexagon> oldHexagons { get => surface.oldhexagons; set => surface.oldhexagons = value; }
    private WorkerSettings workerSettings;

    void Start()
    {
        workerSettings = Settings.Instance.workerSettings;
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
        if (job.type == JobType.DIG)
        {
            StartCoroutine(Dig(job));
        }
        else if (job.type == JobType.FILL)
        {
            StartCoroutine(Fill(job));
        }
    }

    public IEnumerator Dig(WorkerJob workerJob)
    {
        var worker = workerJob.worker;
        var floorHex = workerJob.hex;
        var wallHex = (WorkHexagon)(workerJob.hex.child);
        floorHex.RemoveChildren();
        var scaledBlock = surface.AddDigScaledBlock(floorHex);
        while (scaledBlock != null)
        {
            scaledBlock.work -= workerSettings.CONSTRUCTION_SPEED;
            scaledBlock.transform.localScale = Vector3.one * scaledBlock.work / WorkHexagon.MAX_WORK;
            if (scaledBlock.work <= 0)
            {
                surface.AddGround(floorHex);
                surface.pathGraph.AllowHexagon(floorHex);
                worker.job.Cancel();
                oldHexagons.Remove(floorHex.id);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator Fill(WorkerJob workerJob)
    {
        var worker = workerJob.worker;
        var floorHex = workerJob.hex;
        floorHex.RemoveChildren();
        var scaledBlock = surface.AddFillScaledBlock(floorHex);
        while (scaledBlock != null)
        {
            scaledBlock.work -= workerSettings.CONSTRUCTION_SPEED;
            scaledBlock.transform.localScale = Vector3.one * (1 - scaledBlock.work / WorkHexagon.MAX_WORK);
            if (scaledBlock.work <= 0)
            {
                floorHex.RemoveChildren();
                surface.AddBlock(floorHex);
                worker.job.Cancel();
                oldHexagons.Remove(floorHex.id);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        surface.pathGraph.AllowHexagon(floorHex);
        worker.job.Cancel();
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