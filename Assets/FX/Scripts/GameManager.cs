using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Creator creator;
    public Spawn spawn;
    private Vector3 pos;
    public NavMeshSurface navSurface;
    // public GameObject back;
    public List<Hexagon> digList;
    public List<Hexagon> groundList;
    public List<Hexagon> spawnList;
    public List<Hexagon> wallList;
    public List<Hexagon> buildList;
    public List<Vector3> sideList;
    public List<Hexagon> eatList;

    public Hexagon groundPrefab;
    public Hexagon wallPrefab;
    public Hexagon digPrefab;
    public Hexagon buildPrefab;
    public Hexagon spawnPrefab;
    public Hexagon eatPrefab;

    public List<IAnt> antsList;
    public GameObject antBuilderPrefab;
    public GameObject antDiggerPrefab;

    public List<Enemy> enemyList;
    public Enemy enemyPrefab;


    public Hexagon[] allHex;
    private Hexagon hex;
    public List<int> pushedList;


    void Awake()
    {
        instance = this;
        navSurface = GetComponent<NavMeshSurface>();
        buildList = new List<Hexagon>();
        wallList = new List<Hexagon>();
        digList = new List<Hexagon>();
        groundList = new List<Hexagon>();
        spawnList = new List<Hexagon>();
        eatList = new List<Hexagon>();
        pushedList = new List<int>();
        antsList = new List<IAnt>();

    }

    public void AddStartWall(Hexagon hex)//самые первые стены везде
    {
        allHex[hex.id] = hex;
        wallList.Add(hex);
    }

        public void AddSpawn(int id)//для зоны спауна 
    {
        hex = allHex[id];
        RemoveFromAllHexList(id);
        pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        hex = Instantiate(spawnPrefab, pos, Quaternion.identity, transform);
        hex.id = id;
        spawnList.Add(hex);//здесь будем спаунить муравьев
        groundList.Add(hex);
        allHex[id] = hex;
        navSurface.BuildNavMesh();
    }

    public void AddGround(int id)//при создании земли по краям для спавна врагов
    {
        hex = allHex[id];
        RemoveFromAllHexList(id);
        pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        hex = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
        hex.id = id;
        groundList.Add(hex);
        allHex[id] = hex;
        navSurface.BuildNavMesh();
    }


    public Hexagon TapToDig(int id)//если тапнули стену создаем заготовку для копания 
    {
        hex = allHex[id];
        RemoveFromAllHexList(id);
        pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);//удаляем стену
        Hexagon dig = Instantiate(digPrefab, pos, Quaternion.identity, transform);
        dig.id = id;
        allHex[id] = dig;
        digList.Add(dig);
        navSurface.BuildNavMesh();
        return dig;
    }

    public Hexagon TapToBuild(int id)//если тапнули землю создаем заготовку где потом построим стену
    {
        hex = allHex[id];
        RemoveFromAllHexList(id);
        pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        Hexagon build = Instantiate(buildPrefab, pos, Quaternion.identity, transform);
        build.id = id;
        allHex[id] = build;
        buildList.Add(build);
        navSurface.BuildNavMesh();
        return build;
    }


    public void Dig(int id)//если выкопали
    {
        hex = allHex[id];
        RemoveFromAllHexList(id);
        pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        Hexagon ground = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
        ground.id = id;
        allHex[id] = ground;
        groundList.Add(ground);
        pushedList.Remove(id);
        navSurface.BuildNavMesh();

    }

    public void Build(int id)//если построили
    {
       hex = allHex[id];
        RemoveFromAllHexList(id);
        pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        Hexagon wall = Instantiate(wallPrefab, pos, Quaternion.identity, creator.transform);
        wall.id = id;
        allHex[id] = wall;
        wallList.Add(wall);
        pushedList.Remove(id);
        StartCoroutine(Build_Cor());

    }
    IEnumerator Build_Cor()
    {
        yield return new WaitForEndOfFrame();
        navSurface.BuildNavMesh();
   
    }

    public void AddEat(int id)//если выкопали
    {

        hex = allHex[id];

        RemoveFromAllHexList(id);
        pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        Hexagon ground = Instantiate(eatPrefab, pos, Quaternion.identity, transform);
        ground.id = id;
        allHex[id] = ground;
        groundList.Add(ground);
        pushedList.Remove(id);
        navSurface.BuildNavMesh();

    }
    private void RemoveFromAllHexList(int id)
    {
        hex = allHex[id];
        buildList.Remove(hex);
        wallList.Remove(hex);
        digList.Remove(hex);
        groundList.Remove(hex);
        spawnList.Remove(hex);
        eatList.Remove(hex);
    }


    public void Evacuate(int id)//убираем муравьев из зоны постройки
    {
        Hexagon buildHex = allHex[id];
        float minDist = 1000000;
        float dist;
        Hexagon nearestHex = null;

        for (int i = 0; i < groundList.Count; i++)//находим ближайшую землю
        {
            dist = Vector3.Distance(buildHex.transform.position, groundList[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestHex = groundList[i];
            }
        }


        for (int i = 0; i < antsList.Count; i++)//находим муравьев в радиусе от застраемой земли, перемещаем их туда
            if (antsList[i] != null && Vector3.Distance(buildHex.transform.position, antsList[i]._transform.position) < 6f)
            {
                antsList[i].agent.Warp(nearestHex.transform.position);
            }

        for (int i = 0; i < enemyList.Count; i++)//находим врагов в радиусе от застраемой земли, перемещаем их туда
            if (enemyList[i] != null && Vector3.Distance(buildHex.transform.position, enemyList[i]._transform.position) < 6f)
            {
                enemyList[i].agent.Warp(nearestHex.transform.position);
            }

    }


}
