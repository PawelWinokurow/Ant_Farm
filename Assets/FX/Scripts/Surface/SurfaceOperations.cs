using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;

public class SurfaceOperations : MonoBehaviour
{
    public Surface Surface;
    public Dictionary<string, Hexagon> OldHexagons { get => Surface.OldHexagons; set => Surface.OldHexagons = value; }

    public void StartJobExecution(FloorHexagon hex, Worker worker)
    {
        switch (worker.Job.Type)
        {
            case JobType.DIG:
                StartCoroutine(Dig(hex, worker));
                break;
            case JobType.FILL:
                StartCoroutine(Fill(hex, worker));
                break;
        }
    }
    public void Loading(CollectingHexagon collectingHex, LoadingState state, CarrierJob job)
    {
        var worker = state.Worker;
        var workerMaxCanTake = state.Worker.MaxCarryingWeight - worker.CarryingWeight;
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
            if (worker.CarryingWeight >= state.Worker.MaxCarryingWeight)
            {
                state.Done();
            }
        }
    }
    public void Unloading(BaseHexagon storageHex, UnloadingState state, CarrierJob job)
    {
        var worker = state.Worker;
        var toStore = Mathf.Min((worker.LoadingSpeed * Time.deltaTime), worker.CarryingWeight);
        storageHex.Storage += toStore;
        Debug.Log(storageHex.Storage);
        worker.CarryingWeight -= toStore;
        if (worker.CarryingWeight <= 0)
        {
            worker.CarryingWeight = 0;
            state.Done();
        }
    }

    public IEnumerator Dig(FloorHexagon hex, Worker worker)
    {
        hex.RemoveChildren();
        var oldIcon = OldHexagons[hex.Id];
        var wallHex = Surface.AddBlock(hex).AssignProperties((WorkHexagon)oldIcon);
        while (wallHex != null)
        {
            wallHex.Work -= worker.ConstructionSpeed;
            wallHex.transform.localScale = Vector3.one * wallHex.Work / WorkHexagon.MaxWork;
            if (wallHex.Work <= 0)
            {
                Surface.AddGround(hex);
                Surface.PathGraph.AllowHexagon(wallHex.FloorHexagon);
                OldHexagons.Remove(hex.Id);
                worker.Job.Cancel();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        wallHex = Surface.AddBlock(hex).AssignProperties((WorkHexagon)oldIcon);
        OldHexagons.Remove(hex.Id);
        worker.Job.Cancel();
    }

    public IEnumerator Fill(FloorHexagon hex, Worker worker)
    {
        hex.RemoveChildren();
        // OldHexagons.Remove(hex.Id);
        var scaledBlock = Surface.AddScaledBlock(hex);
        while (scaledBlock != null)
        {
            scaledBlock.Work -= worker.ConstructionSpeed;
            scaledBlock.transform.localScale = Vector3.one * (1 - scaledBlock.Work / WorkHexagon.MaxWork);
            if (scaledBlock.Work <= 0)
            {
                hex.RemoveChildren();
                Surface.AddBlock(hex);
                worker.Job.Cancel();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        // AddGround(hex);
        Surface.PathGraph.AllowHexagon(hex);
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