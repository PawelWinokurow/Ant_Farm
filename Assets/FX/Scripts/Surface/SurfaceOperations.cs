using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class SurfaceOperations : MonoBehaviour
{
    public Surface Surface;
    public Dictionary<string, Hexagon> OldHexagons { get => Surface.OldHexagons; set => Surface.OldHexagons = value; }

    public void Loading(CollectingHexagon collectingHex, LoadingState state, CarrierJob job)
    {
        var worker = state.worker;
        var workerMaxCanTake = state.worker.MaxCarryingWeight - worker.CarryingWeight;
        var canTake = Mathf.Min((worker.LoadingSpeed * Time.deltaTime), workerMaxCanTake);
        if (canTake >= collectingHex.Quantity)
        {
            worker.CarryingWeight += collectingHex.Quantity;
            collectingHex.Quantity = 0;
            collectingHex.Carriers.Where(w => w.Id != worker.Id).ToList().ForEach(w => w.CancelJob());
            collectingHex.Type = HEX_TYPE.EMPTY;
            collectingHex.FloorHexagon.RemoveChildren();
            state.Done();
        }
        else
        {
            var toTake = Mathf.Min(canTake, workerMaxCanTake);
            worker.CarryingWeight += toTake;
            collectingHex.Quantity -= toTake;
            if (worker.CarryingWeight >= state.worker.MaxCarryingWeight)
            {
                state.Done();
            }
        }
    }
    public void Unloading(BaseHexagon storageHex, UnloadingState state, CarrierJob job)
    {
        var worker = state.worker;
        var toStore = Mathf.Min((worker.LoadingSpeed * Time.deltaTime), worker.CarryingWeight);
        storageHex.Storage += toStore;
        worker.CarryingWeight -= toStore;
        if (worker.CarryingWeight <= 0)
        {
            worker.CarryingWeight = 0;
            state.Done();
        }
    }

    public void Build(WorkerJob job)
    {
        if (job.Type == JobType.DIG)
        {
            StartCoroutine(Dig(job));
        }
        else if (job.Type == JobType.FILL)
        {
            StartCoroutine(Fill(job));
        }
    }

    public IEnumerator Dig(WorkerJob workerJob)
    {
        var worker = workerJob.Worker;
        var floorHex = workerJob.Hex;
        var wallHex = (WorkHexagon)(workerJob.Hex.Child);
        floorHex.RemoveChildren();
        var scaledBlock = Surface.AddDigScaledBlock(floorHex);
        while (scaledBlock != null)
        {
            scaledBlock.Work -= worker.ConstructionSpeed;
            scaledBlock.transform.localScale = Vector3.one * scaledBlock.Work / WorkHexagon.MaxWork;
            if (scaledBlock.Work <= 0)
            {
                Surface.AddGround(floorHex);
                Surface.PathGraph.AllowHexagon(wallHex.FloorHexagon);
                worker.Job.Cancel();
                OldHexagons.Remove(floorHex.Id);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator Fill(WorkerJob workerJob)
    {
        var worker = workerJob.Worker;
        var floorHex = workerJob.Hex;
        var wallHex = (WorkHexagon)(workerJob.Hex.Child);

        floorHex.RemoveChildren();
        var scaledBlock = Surface.AddFillScaledBlock(floorHex);
        while (scaledBlock != null)
        {
            scaledBlock.Work -= worker.ConstructionSpeed;
            scaledBlock.transform.localScale = Vector3.one * (1 - scaledBlock.Work / WorkHexagon.MaxWork);
            if (scaledBlock.Work <= 0)
            {
                floorHex.RemoveChildren();
                Surface.AddBlock(floorHex);
                worker.Job.Cancel();
                OldHexagons.Remove(floorHex.Id);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        Surface.PathGraph.AllowHexagon(floorHex);
        worker.Job.Cancel();
    }

    public bool IsInOldHexagons(FloorHexagon hex)
    {
        return OldHexagons.ContainsKey(hex.Id);
    }

    public BaseHexagon NearestBaseHexagon()
    {
        //TODO implement
        return (BaseHexagon)Surface.BaseHex.Child;
    }

}