using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobFactory : MonoBehaviour
{
    private int id = 0;
    public WorkerJobScheduler workerJobScheduler;
    public EnemyJobScheduler enemyJobScheduler;
    public Surface surface;
    public SurfaceOperations surfaceOperations;
    public Worker workerPrefab;
    public Enemy enemyPrefab;
    public List<Mob> allMobs = new List<Mob>();
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
        Worker worker = Instantiate(workerPrefab, surface.PositionToHex(surface.baseHex.position + new Vector3(4, 0, 6)).position, Quaternion.identity);
        worker.id = id;
        workerJobScheduler.AddWorker(worker);
        enemyJobScheduler.AddWorker(worker);
        allMobs.Add(worker);
        yield break;
    }
    IEnumerator SpawnEnemy(string id)
    {
        Enemy enemy = Instantiate(enemyPrefab, surface.PositionToHex(surface.baseHex.position + new Vector3(10, 0, 20)).position, Quaternion.identity);
        enemy.id = id;
        enemy.allMobs = allMobs;
        //TODO remove 
        enemyJobScheduler.AddEnemy(enemy);
        yield break;
    }
}
