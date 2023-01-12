using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storyteller : MonoBehaviour
{

    private Surface surface;
    private Store store;
    private PriceSettings priceSettings;
    private Vector3 holeTestPosition = new Vector3(51.26929f, 0, 154.7541f);
    private FloorHexagon holeHex;
    private WorkHexagon hole;
    private Pricing pricing;
    public MobFactory mobFactory;
    private int id = 1000;

    void Start()
    {
        surface = Surface.Instance;
        store = Store.Instance;
        pricing = Pricing.Instance;
        holeHex = surface.PositionToHex(holeTestPosition);
        priceSettings = Settings.Instance.priceSettings;
        StartSpawner();
    }

    void StartSpawner()
    {
        StartCoroutine(SpawnHole());
    }

    List<Func<string, FloorHexagon, IEnumerator>> CalculateEnemiesBatch()
    {
        var overallColonyCost = pricing.colonyCost + store.food;
        //TODO better solution 
        var spawnFuncsToChoose = new List<Func<string, FloorHexagon, IEnumerator>>() { mobFactory.SpawnBlob, mobFactory.SpawnGobber, mobFactory.SpawnScorpion, mobFactory.SpawnZombie };
        var spawnFuncs = new List<Func<string, FloorHexagon, IEnumerator>>() { mobFactory.SpawnScorpion };
        overallColonyCost -= priceSettings.SCORPION_PRICE;
        while (overallColonyCost >= 0)
        {
            int index = UnityEngine.Random.Range(0, spawnFuncsToChoose.Count);
            var enemySpawnFunc = spawnFuncsToChoose[index];
            spawnFuncs.Add(enemySpawnFunc);
            overallColonyCost -= pricing.pricesBySpawnFunc[enemySpawnFunc];
        }
        return spawnFuncs;
    }
    IEnumerator SpawnHole()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            hole = surface.AddBlock(holeHex, HexType.HOLE);
            StartCoroutine(SpawnEnemies());
            yield return new WaitForSeconds(30f);
        }
    }

    IEnumerator SpawnEnemies()
    {
        var spawnFuncs = CalculateEnemiesBatch();
        while (spawnFuncs.Count > 0)
        {
            yield return new WaitForSeconds(0.2f);
            var spawnFunc = spawnFuncs[0];
            spawnFuncs.RemoveAt(0);
            StartCoroutine(spawnFunc($"enemy{id++}", holeHex));
        }
        surface.ClearHex(holeHex);
        surface.AddGround(holeHex);
    }
}