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
    private GameSettings gameSettings;
    private int id = 1000;
    public UIManager uiManager;


    void Start()
    {
        surface = Surface.Instance;
        store = Store.Instance;
        pricing = Pricing.Instance;
        gameSettings = Settings.Instance.gameSettings;
        priceSettings = Settings.Instance.priceSettings;
        holeHex = surface.PositionToHex(holeTestPosition);
    }


    List<(Func<string, FloorHexagon, IEnumerator>, int)> CalculateEnemiesBatch()
    {
        var overallColonyCost = pricing.colonyCost + store.food;
        //TODO better solution 
        var spawnFuncsToChoose = new List<(Func<string, FloorHexagon, IEnumerator>, int)>() {
            (mobFactory.SpawnGobber, 1),
            (mobFactory.SpawnScorpion, 0),
            (mobFactory.SpawnBlob, 2),
            (mobFactory.SpawnZombie, 3)
        };
        var enemySpawnTuples = new List<(Func<string, FloorHexagon, IEnumerator>, int)>() { (mobFactory.SpawnScorpion, 0) };
        overallColonyCost -= priceSettings.SCORPION_PRICE;
        while (overallColonyCost >= 0)
        {
            int index = UnityEngine.Random.Range(0, spawnFuncsToChoose.Count);
            var enemySpawnTuple = spawnFuncsToChoose[index];
            enemySpawnTuples.Add(enemySpawnTuple);
            overallColonyCost -= pricing.pricesBySpawnFunc[enemySpawnTuple.Item1];
        }
        return enemySpawnTuples;
    }
    public void SpawnHole()
    {
        surface.ClearHex(holeHex);
        surface.AddBlock(holeHex, HexType.HOLE);
        surface.pathGraph.SetAccesabillity(holeHex, gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
        holeHex.vertex.neighbours.ForEach(vertex =>
        {
            surface.RemoveBuilding(vertex.floorHexagon);
        });
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        var spawnTuples = CalculateEnemiesBatch();
        int[] spawnIcons = Enumerable.Repeat(0, 4).ToArray();
        spawnTuples.ForEach(tuple => spawnIcons[tuple.Item2]++);
        uiManager.EnemySpawnIcons(spawnIcons);
        while (spawnTuples.Count > 0)
        {
            yield return new WaitForSeconds(0.2f);
            var spawnFunc = spawnTuples[0].Item1;
            spawnTuples.RemoveAt(0);
            StartCoroutine(spawnFunc($"enemy{id++}", holeHex));
        }
        surface.RemoveBuilding(holeHex);
    }
}