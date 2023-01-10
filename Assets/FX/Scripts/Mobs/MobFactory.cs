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
    private Surface surface;
    private SurfaceOperations surfaceOperations;
    private Store store;
    public Worker workerPrefab;
    public Scorpion scorpionPrefab;
    public Zombie zombiePrefab;
    public Gobber gobberPrefab;
    public Gunner gunnerPrefab;
    public Soldier soldierPrefab;
    public Blob blobPrefab;
    private PriceSettings priceSettings;

    void Start()
    {
        pathfinder = workerJobScheduler.pathfinder;
        priceSettings = Settings.Instance.priceSettings;
        surface = Surface.Instance;
        surfaceOperations = SurfaceOperations.Instance;
        store = Store.Instance;
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

    public IEnumerator SpawnScorpion(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Scorpion scorpion = Instantiate(scorpionPrefab, spawnPosition, Quaternion.identity);
        scorpion.id = id;
        scorpion.pathfinder = pathfinder;
        scorpion.Kill = () =>
        {
            store.DeleteEnemy((Targetable)scorpion);
            scorpion.SetState(new FighterNamespace.DeadState(scorpion));
            scorpion.CancelJob();
        };
        scorpion.type = ACTOR_TYPE.SCORPION;
        store.AddEnemy((Targetable)scorpion);
        yield return null;
    }

    public IEnumerator SpawnZombie(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Zombie zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        zombie.id = id;
        zombie.pathfinder = pathfinder;
        zombie.Kill = () =>
        {
            store.DeleteEnemy((Targetable)zombie);
            zombie.SetState(new FighterNamespace.DeadState(zombie));
            zombie.CancelJob();
        };
        zombie.MutateMob = AddMobByType;
        zombie.type = ACTOR_TYPE.ZOMBIE;
        store.AddEnemy((Targetable)zombie);
        yield return null;
    }

    public IEnumerator SpawnBlob(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Blob blob = Instantiate(blobPrefab, spawnPosition, Quaternion.identity);
        blob.id = id;
        blob.pathfinder = pathfinder;
        blob.Kill = () =>
        {
            store.DeleteEnemy((Targetable)blob);
            blob.SetState(new FighterNamespace.DeadState(blob));
            blob.CancelJob();
        };
        blob.type = ACTOR_TYPE.BLOB;
        store.AddEnemy((Targetable)blob);
        yield return null;
    }

    public IEnumerator SpawnGobber(string id, FloorHexagon hex)
    {
        var spawnPosition = hex.position;
        Gobber gobber = Instantiate(gobberPrefab, spawnPosition, Quaternion.identity);
        gobber.id = id;
        gobber.pathfinder = pathfinder;
        gobber.Kill = () =>
        {
            store.DeleteEnemy((Targetable)gobber);
            gobber.SetState(new FighterNamespace.DeadState(gobber));
            gobber.CancelJob();
        };
        gobber.type = ACTOR_TYPE.GOBBER;
        store.AddEnemy((Targetable)gobber);
        yield return null;
    }
    public IEnumerator SpawnWorker(string id, FloorHexagon hex)
    {
        if (store.food >= priceSettings.WORKER_PRICE)
        {
            var spawnPosition = hex.position;
            Worker worker = Instantiate(workerPrefab, spawnPosition, Quaternion.identity);
            worker.id = id;
            worker.type = ACTOR_TYPE.WORKER;
            workerJobScheduler.AddWorker(worker);
            store.AddAlly(worker);
        }
        yield return null;
    }
    public IEnumerator SpawnSoldier(string id, FloorHexagon hex)
    {
        if (store.food >= priceSettings.SOLDIER_PRICE)
        {
            var spawnPosition = hex.position;
            Soldier soldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
            soldier.id = id;
            soldier.pathfinder = pathfinder;
            soldier.Kill = () =>
            {
                store.DeleteAlly((Targetable)soldier);
                soldier.SetState(new FighterNamespace.DeadState(soldier));
                soldier.CancelJob();
            };
            soldier.type = ACTOR_TYPE.SOLDIER;
            store.AddAlly((Targetable)soldier);
        }
        yield return null;
    }
    public IEnumerator SpawnGunner(string id, FloorHexagon hex)
    {
        if (store.food >= priceSettings.GUNNER_PRICE)
        {
            var spawnPosition = hex.position;
            Gunner gunner = Instantiate(gunnerPrefab, spawnPosition, Quaternion.identity);
            gunner.id = id;
            gunner.pathfinder = pathfinder;
            gunner.Kill = () =>
            {
                store.DeleteAlly((Targetable)gunner);
                gunner.SetState(new FighterNamespace.DeadState(gunner));
                gunner.CancelJob();
            };
            gunner.type = ACTOR_TYPE.GUNNER;
            store.AddAlly((Targetable)gunner);
        }
        yield return null;
    }

}
