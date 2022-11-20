using System.Collections;
using UnityEngine;


public class MobFactory : MonoBehaviour
{
    private int id = 0;
    public WorkerJobScheduler workerJobScheduler;
    public Surface surface;
    public SurfaceOperations surfaceOperations;
    public Worker workerPrefab;
    public Enemy enemyPrefab;
    public Store store;
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
        var baseNeighbours = surface.baseHex.vertex.neighbours;
        var spawnPosition = baseNeighbours[UnityEngine.Random.Range(0, baseNeighbours.Count)].position;
        Worker worker = Instantiate(workerPrefab, spawnPosition, Quaternion.identity);
        worker.id = id;
        // worker.currentHex = surface.PositionToHex(spawnPosition);
        workerJobScheduler.AddWorker(worker);
        store.AddMob(worker);
        yield break;
    }
    IEnumerator SpawnEnemy(string id)
    {
        var spawnPosition = surface.PositionToHex(surface.baseHex.position + new Vector3(10, 0, 20)).position;
        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.id = id;
        // enemy.currentHex = surface.PositionToHex(spawnPosition);
        enemy.pathfinder = workerJobScheduler.pathfinder;
        enemy.store = store;
        yield break;
    }
}
