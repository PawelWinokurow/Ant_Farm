using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class FX_Manager : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        [HideInInspector]
        public string tag;
        public GameObject prefab;
        [HideInInspector]
        public List<GameObject> fxList;
    }

    public List<Pool> pools;

    public static FX_Manager instance;
    public Dictionary<string, Pool> poolDictionary;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Pool>();
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        string tag = prefab.name;
        Pool pool;
        if (!poolDictionary.ContainsKey(tag))
        {
            pool = new Pool()
            {
                tag = tag,
                prefab = prefab,
                fxList = new List<GameObject>()
            };


            poolDictionary.Add(tag, pool);
            pools.Add(pool);
        }

        pool = poolDictionary[tag];
        GameObject go = null;

        pool.fxList = pool.fxList.Where(fx => fx != null).ToList();

        var index = pool.fxList.FindIndex(fx => !fx.gameObject.activeInHierarchy);
        if (index == -1)
        {
            go = Instantiate(pool.prefab);
            pool.fxList.Add(go);
            go.transform.parent = transform;
        }
        else
        {
            go = pool.fxList[index];
        }

        go.SetActive(false);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = pool.prefab.transform.localScale;
        go.SetActive(true);
        if (parent != null)
        {
            go.transform.parent = parent;
        }
        return go;
    }

}