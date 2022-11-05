using System;
using System.Collections;
using UnityEngine;


public class MobFactory : MonoBehaviour
{
    public JobScheduler JobScheduler;
    public Surface Surface;
    public Worker WorkerPrefab;
    public void AddMob()
    {
        for (int i = 0; i < 100; i++)
        {
            StartCoroutine(SpawnWorker());
        }
    }

    IEnumerator SpawnWorker()
    {
        JobScheduler.AddMob(Instantiate(WorkerPrefab, Surface.BaseHex.Position + new Vector3(5, 0, 5), Quaternion.identity));
        yield break;
    }
}
