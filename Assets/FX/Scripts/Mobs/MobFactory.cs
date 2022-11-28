using System.Collections;
using UnityEngine;
using EnemyNamespace;
using WorkerNamespace;
using SoldierNamespace;

public class MobFactory : MonoBehaviour
{
    private int id = 0;
    public WorkerJobScheduler workerJobScheduler;
    private Pathfinder pathfinder;
    public Surface surface;
    public SurfaceOperations surfaceOperations;
    public Worker workerPrefab;
    public Enemy enemyPrefab;
    public Soldier soldierPrefab;
    public Store store;

    void Start()
    {
        pathfinder = workerJobScheduler.pathfinder;
    }
    public void AddWorker(FloorHexagon hex)
    {
        StartCoroutine(SpawnWorker($"worker_{id}", hex));
    }
    public void AddEnemy()
    {
        StartCoroutine(SpawnEnemy($"enemy_{id}"));
    }
    public void AddSoldier(FloorHexagon hex)
    {
        StartCoroutine(SpawnSoldier($"soldier_{id}", hex));
    }

    IEnumerator SpawnWorker(string id, FloorHexagon hex)
    {
        var baseNeighbours = surface.baseHex.vertex.neighbours;
        var spawnPosition = hex.position;
        Worker worker = Instantiate(workerPrefab, spawnPosition, Quaternion.identity);
        worker.id = id;
        workerJobScheduler.AddWorker(worker);
        store.AddMob(worker);
        yield return null;
    }
    IEnumerator SpawnEnemy(string id)
    {
        var spawnHex = surface.PositionToHex(surface.baseHex.position + new Vector3(10, 0, 20));
        var spawnPosition = surface.PositionToHex(surface.baseHex.position + new Vector3(10, 0, 20)).position;
        spawnHex.vertex.neighbours.ForEach(n => surface.AddGround(n.floorHexagon));
        surface.AddGround(spawnHex);
        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.id = id;
        enemy.pathfinder = pathfinder;
        enemy.store = store;
        store.AddEnemy(enemy);
        yield return null;
    }
    IEnumerator SpawnSoldier(string id, FloorHexagon hex)
    {
        var baseNeighbours = surface.baseHex.vertex.neighbours;
        var spawnPosition = hex.position;
        Soldier soldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
        soldier.id = id;
        soldier.pathfinder = pathfinder;
        soldier.store = store;
        store.AddMob(soldier);
        yield return null;
    }
}
