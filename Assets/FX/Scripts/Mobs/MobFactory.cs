using System;
using System.Collections;
using FighterNamespace;
using UnityEngine;
using WorkerNamespace;

public class MobFactory : MonoBehaviour
{
    private int id = 0;
    public WorkerJobScheduler workerJobScheduler;
    private Pathfinder pathfinder;
    public Surface surface;
    public SurfaceOperations surfaceOperations;
    public Worker workerPrefab;
    public Scorpion scorpionPrefab;
    public Zombie zombiePrefab;
    public Gobber gobberPrefab;
    public Gunner gunnerPrefab;
    public Soldier soldierPrefab;
    public Blob blobPrefab;
    public Store store;
    public PopUp popUp_prefad;

    void Start()
    {
        pathfinder = workerJobScheduler.pathfinder;
    }
    public void AddMobByType(SliderValue type, FloorHexagon hex)
    {
        Func<string, FloorHexagon, IEnumerator> action = null;
        if (type == SliderValue.WORKER) action = SpawnWorker;
        else if (type == SliderValue.GOBBER) action = SpawnGobber;
        else if (type == SliderValue.GUNNER) action = SpawnGunner;
        else if (type == SliderValue.SCORPION) action = SpawnScorpion;
        else if (type == SliderValue.SOLDIER) action = SpawnSoldier;
        else if (type == SliderValue.ZOMBIE) action = SpawnZombie;
        else if (type == SliderValue.BLOB) action = SpawnBlob;
        StartCoroutine(action($"{type}_{id++}", hex));
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
            store.DeleteEnemy((Targetable)scorpion);
            scorpion.SetState(new FighterNamespace.DeadState(scorpion));
            scorpion.CancelJob();
        };
        store.AddEnemy((Targetable)scorpion);
        Buy(scorpion, 10);
        yield return null;
    }

    IEnumerator SpawnZombie(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Zombie zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        zombie.id = id;
        zombie.pathfinder = pathfinder;
        zombie.store = store;
        zombie.surfaceOperations = surfaceOperations;
        zombie.Kill = () =>
        {
            store.DeleteEnemy((Targetable)zombie);
            zombie.SetState(new FighterNamespace.DeadState(zombie));
            zombie.CancelJob();
        };
        zombie.MutateMob = AddMobByType;
        store.AddEnemy((Targetable)zombie);
        Buy(zombie, 10);
        yield return null;
    }

    IEnumerator SpawnBlob(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Blob blob = Instantiate(blobPrefab, spawnPosition, Quaternion.identity);
        blob.id = id;
        blob.pathfinder = pathfinder;
        blob.store = store;
        blob.surfaceOperations = surfaceOperations;
        blob.Kill = () =>
        {
            store.DeleteEnemy((Targetable)blob);
            blob.SetState(new FighterNamespace.DeadState(blob));
            blob.CancelJob();
        };
        store.AddEnemy((Targetable)blob);
        Buy(blob, 10);
        yield return null;
    }

    IEnumerator SpawnGobber(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Gobber gobber = Instantiate(gobberPrefab, spawnPosition, Quaternion.identity);
        gobber.id = id;
        gobber.pathfinder = pathfinder;
        gobber.store = store;
        gobber.surfaceOperations = surfaceOperations;
        gobber.Kill = () =>
        {
            store.DeleteEnemy((Targetable)gobber);
            gobber.SetState(new FighterNamespace.DeadState(gobber));
            gobber.CancelJob();
        };
        store.AddEnemy((Targetable)gobber);
        Buy(gobber, 10);
        yield return null;
    }
    IEnumerator SpawnWorker(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Worker worker = Instantiate(workerPrefab, spawnPosition, Quaternion.identity);
        worker.id = id;
        worker.store = store;
        workerJobScheduler.AddWorker(worker);
        store.AddAlly(worker);
        Buy(worker, 10);
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
            store.DeleteAlly((Targetable)soldier);
            soldier.SetState(new FighterNamespace.DeadState(soldier));
            soldier.CancelJob();
        };
        store.AddAlly((Targetable)soldier);
        Buy(soldier, 10);
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
            store.DeleteAlly((Targetable)gunner);
            gunner.SetState(new FighterNamespace.DeadState(gunner));
            gunner.CancelJob();
        };
        store.AddAlly((Targetable)gunner);
        Buy(gunner, 10);
        yield return null;
    }

    public void Buy(Mob mob, int cost)
    {
        PopUp popUp = Instantiate(popUp_prefad, mob.position, Quaternion.identity, transform);
        popUp.tmp.text = (-cost).ToString();
    }
}
