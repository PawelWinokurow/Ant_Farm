using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        int i;

        for (i = 0; i < pool.fxList.Count; i++)
        {
            go = pool.fxList[i];

            if (!go.gameObject.activeInHierarchy)
            {
                break;
            }
        }

        if (i == pool.fxList.Count || go == null)
        {
            go = Instantiate(pool.prefab);
            pool.fxList.Add(go);
            go.transform.parent = transform;
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