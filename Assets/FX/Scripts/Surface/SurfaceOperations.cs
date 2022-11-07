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
            case JobType.CARRYING:
                Carrying(hex, worker);
                break;
        }
    }


    public void Carrying(FloorHexagon hex, Worker worker)
    {
        var job = (CarrierJob)worker.Job;
        if (job.Direction == Direction.COLLECTING)
        {
            StartCoroutine(Loading((CollectingHexagon)(hex.Child), (BaseHexagon)(Surface.BaseHex.Child), worker, job));
        }
        else if (job.Direction == Direction.STORAGE)
        {
            StartCoroutine(Unloading((CollectingHexagon)(hex.Child), (BaseHexagon)(Surface.BaseHex.Child), worker, job));
        }
    }

    private IEnumerator Loading(CollectingHexagon collectingHexagon, BaseHexagon baseHexagon, Worker worker, CarrierJob job)
    {
        var workerCanCarry = worker.MaxCarryingWeight - worker.CarryingWeight;
        if (workerCanCarry >= collectingHexagon.Quantity)
        {
            worker.CarryingWeight = collectingHexagon.Quantity;
            collectingHexagon.Quantity = 0;
            collectingHexagon.Carriers.Where(w => w.Id != worker.Id && w.CurrentState.Type == STATE.GOTO).ToList().ForEach(w => w.Job.CancelJob());
            collectingHexagon.Type = HEX_TYPE.EMPTY;
            yield return new WaitForSeconds(2f);
            collectingHexagon.FloorHexagon.RemoveChildren();
        }
        else
        {
            worker.CarryingWeight = workerCanCarry;
            collectingHexagon.Quantity -= workerCanCarry;
            yield return new WaitForSeconds(2f);
            collectingHexagon.transform.localScale = Vector3.one * collectingHexagon.Quantity / CollectingHexagon.MaxQuantity;
            collectingHexagon.transform.Rotate(0f, 30f, 0f, Space.Self);
        }
        job.Return();
    }
    private IEnumerator Unloading(CollectingHexagon collectingHexagon, BaseHexagon baseHexagon, Worker worker, CarrierJob job)
    {
        baseHexagon.Storage += worker.CarryingWeight;
        worker.CarryingWeight = 0;
        yield return new WaitForSeconds(2f);
        if (collectingHexagon.Type == HEX_TYPE.FOOD)
        {
            job.Return();
        }
        else
        {
            job.CancelJob();
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
                worker.Job.CancelJob();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        wallHex = Surface.AddBlock(hex).AssignProperties((WorkHexagon)oldIcon);
        OldHexagons.Remove(hex.Id);
        worker.Job.CancelJob();
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
                worker.Job.CancelJob();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        // AddGround(hex);
        Surface.PathGraph.AllowHexagon(hex);
        worker.Job.CancelJob();
    }

    public bool IsInOldHexagons(FloorHexagon hex)
    {
        return OldHexagons.ContainsKey(hex.Id);
    }

}