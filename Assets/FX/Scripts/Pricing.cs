using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pricing : MonoBehaviour
{
    private Store store;
    PriceSettings priceSettings;
    public Dictionary<ACTOR_TYPE, int> pricesByActorType;
    public Dictionary<SliderValue, int> pricesBySliderValue;
    public Dictionary<HexType, int> pricesByHexType;
    public Dictionary<Func<string, FloorHexagon, IEnumerator>, int> pricesBySpawnFunc;
    public MobFactory mobFactory;
    public static Pricing Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        store = Store.Instance;
        priceSettings = Settings.Instance.priceSettings;
        pricesByActorType = new Dictionary<ACTOR_TYPE, int>(){
        {ACTOR_TYPE.BLOB, priceSettings.BLOB_PRICE},
        {ACTOR_TYPE.GOBBER, priceSettings.GOBBER_PRICE},
        {ACTOR_TYPE.SCORPION, priceSettings.SCORPION_PRICE},
        {ACTOR_TYPE.ZOMBIE, priceSettings.ZOMBIE_PRICE},
        {ACTOR_TYPE.TURRET, priceSettings.TURRET_PRICE},
        {ACTOR_TYPE.SOLDIER, priceSettings.SOLDIER_PRICE},
        {ACTOR_TYPE.GUNNER, priceSettings.GUNNER_PRICE},
        {ACTOR_TYPE.WORKER, priceSettings.WORKER_PRICE}
        };


        pricesBySliderValue = new Dictionary<SliderValue, int>(){
        {SliderValue.BLOB, priceSettings.BLOB_PRICE},
        {SliderValue.GOBBER, priceSettings.GOBBER_PRICE},
        {SliderValue.SCORPION, priceSettings.SCORPION_PRICE},
        {SliderValue.ZOMBIE, priceSettings.ZOMBIE_PRICE},
        {SliderValue.TURRET, priceSettings.TURRET_PRICE},
        {SliderValue.SOLDIER, priceSettings.SOLDIER_PRICE},
        {SliderValue.GUNNER, priceSettings.GUNNER_PRICE},
        {SliderValue.WORKER, priceSettings.WORKER_PRICE},
        {SliderValue.SOIL, priceSettings.SOIL_PRICE},
        {SliderValue.STONE, priceSettings.STONE_PRICE},
        {SliderValue.SPIKES, priceSettings.SPIKES_PRICE}
        };

        pricesByHexType = new Dictionary<HexType, int>(){
        {HexType.SPIKES, priceSettings.SPIKES_PRICE},
        {HexType.TURRET, priceSettings.TURRET_PRICE},
        {HexType.STONE, priceSettings.STONE_PRICE},
        {HexType.SOIL, priceSettings.SOIL_PRICE},
        };

        pricesBySpawnFunc = new Dictionary<Func<string, FloorHexagon, IEnumerator>, int>()
        {
            {mobFactory.SpawnBlob, priceSettings.BLOB_PRICE},
            { mobFactory.SpawnGobber, priceSettings.GOBBER_PRICE},
            { mobFactory.SpawnScorpion, priceSettings.SCORPION_PRICE},
            { mobFactory.SpawnZombie, priceSettings.ZOMBIE_PRICE},
        };
    }

    public int colonyCost { get => store.allAllies.Aggregate(0, (acc, targetable) => acc + pricesByActorType[targetable.type]); }
}
