using System;
using System.Collections;
using UnityEngine;


public class MobFactory : MonoBehaviour
{
    public JobScheduler JobScheduler;
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
        JobScheduler.AddMob(Instantiate(WorkerPrefab, new Vector3(30, 0, 30), Quaternion.identity));
        yield break;
    }
}
