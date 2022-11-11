using System;
using System.Collections;
using UnityEngine;


public class MobFactory : MonoBehaviour
{
    private int id = 0;
    public WorkerJobScheduler WorkerJobScheduler;
    public EnemyJobScheduler EnemyJobScheduler;
    public Surface Surface;
    public SurfaceOperations SurfaceOperations;
    public Worker WorkerPrefab;
    public Enemy EnemyPrefab;
    public void AddWorker()
    {
        for (int i = 0; i < 1; i++)
        {
            id++;
            StartCoroutine(SpawnWorker($"worker_{id}"));
        }
    }
    public void AddEnemy()
    {
        for (int i = 0; i < 1; i++)
        {
            id++;
            StartCoroutine(SpawnEnemy($"enemy_{id}"));
        }
    }

    IEnumerator SpawnWorker(string id)
    {
        Worker worker = Instantiate(WorkerPrefab, Surface.PositionToHex(Surface.BaseHex.Position + new Vector3(4, 0, 6)).Position, Quaternion.identity);
        worker.Id = id;
        WorkerJobScheduler.AddWorker(worker);
        EnemyJobScheduler.AddWorker(worker);
        yield break;
    }
    IEnumerator SpawnEnemy(string id)
    {
        Enemy enemy = Instantiate(EnemyPrefab, Surface.PositionToHex(Surface.BaseHex.Position + new Vector3(10, 0, 20)).Position, Quaternion.identity);
        enemy.Id = id;
        EnemyJobScheduler.AddEnemy(enemy);
        yield break;
    }
}
