using System;
using System.Collections;
using UnityEngine;


public class MobFactory : MonoBehaviour
{
    private int id = 0;
    public WorkerJobScheduler WorkerJobScheduler;
    public Surface Surface;
    public SurfaceOperations SurfaceOperations;
    public Worker WorkerPrefab;
    public void AddMob()
    {
        for (int i = 0; i < 1; i++)
        {
            id++;
            StartCoroutine(SpawnWorker($"{id}"));
        }
    }

    IEnumerator SpawnWorker(string id)
    {
        Worker worker = Instantiate(WorkerPrefab, Surface.PositionToHex(Surface.BaseHex.Position + new Vector3(4, 0, 6)).Position, Quaternion.identity);
        worker.Id = id;
        WorkerJobScheduler.AddMob(worker);
        yield break;
    }
}
