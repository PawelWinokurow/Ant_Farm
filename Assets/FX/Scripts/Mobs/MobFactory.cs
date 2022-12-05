using System.Collections;
using UnityEngine;
using ScorpionNamespace;
using WorkerNamespace;
using SoldierNamespace;
using GunnerNamespace;

public class MobFactory : MonoBehaviour
{
    private int id = 0;
    public WorkerJobScheduler workerJobScheduler;
    private Pathfinder pathfinder;
    public Surface surface;
    public SurfaceOperations surfaceOperations;
    public Worker workerPrefab;
    public Scorpion scorpionPrefab;
    public Gunner gunnerPrefab;
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
    public void AddScorpion(FloorHexagon hex)
    {
        StartCoroutine(SpawnScorpion($"scorpion_{id}", hex));
    }
    public void AddGunner(FloorHexagon hex)
    {
        StartCoroutine(SpawnGunner($"gunner{id}", hex));
    }
    public void AddSoldier(FloorHexagon hex)
    {
        StartCoroutine(SpawnSoldier($"soldier_{id}", hex));
    }

    IEnumerator SpawnScorpion(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Scorpion scorpion = Instantiate(scorpionPrefab, spawnPosition, Quaternion.identity);
        scorpion.id = id;
        scorpion.pathfinder = pathfinder;
        scorpion.store = store;
        scorpion.surfaceOperations = surfaceOperations;
        scorpion.Kill = () =>
        {
            store.DeleteEnemy(scorpion);
            scorpion.SetState(new DeadState(scorpion));
            scorpion.CancelJob();
        };
        store.AddEnemy(scorpion);

        yield return null;
    }
    IEnumerator SpawnWorker(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Worker worker = Instantiate(workerPrefab, spawnPosition, Quaternion.identity);
        worker.id = id;
        workerJobScheduler.AddWorker(worker);
        store.AddAlly(worker);
        yield return null;
    }
    IEnumerator SpawnSoldier(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Soldier soldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
        soldier.id = id;
        soldier.pathfinder = pathfinder;
        soldier.store = store;
        soldier.surfaceOperations = surfaceOperations;
        soldier.Kill = () =>
        {
            store.DeleteAlly(soldier);
            soldier.SetState(new DeadState(soldier));
            soldier.CancelJob();
        };
        store.AddAlly(soldier);
        yield return null;
    }
    IEnumerator SpawnGunner(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Gunner gunner = Instantiate(gunnerPrefab, spawnPosition, Quaternion.identity);
        gunner.id = id;
        gunner.pathfinder = pathfinder;
        gunner.store = store;
        gunner.surfaceOperations = surfaceOperations;
        gunner.Kill = () =>
        {
            store.DeleteAlly(gunner);
            gunner.SetState(new DeadState(gunner));
            gunner.CancelJob();
        };
        store.AddAlly(gunner);
        yield return null;
    }
}
