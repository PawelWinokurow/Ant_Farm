using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Surface : MonoBehaviour
{

    public Creator creator;
    private Vector3 pos;
    public NavMeshSurface navSurface;
    public List<Hexagon> digList;
    public List<Hexagon> groundList;
    public List<Hexagon> spawnList;
    public List<Hexagon> wallList;
    public List<Hexagon> buildList;
    public List<Vector3> sideList;
    public List<int> pushedList;

    public Hexagon groundPrefab;
    public Hexagon wallPrefab;
    public Hexagon digPrefab;
    public Hexagon buildPrefab;
    public Hexagon spawnPrefab;


    public Hexagon[] allHex;
    private Hexagon hex;

    void Awake()
    {
        navSurface = GetComponent<NavMeshSurface>();
        buildList = new List<Hexagon>();
        wallList = new List<Hexagon>();
        digList = new List<Hexagon>();
        groundList = new List<Hexagon>();
        spawnList = new List<Hexagon>();
        pushedList = new List<int>();
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

    public Hexagon AssignDigHex(int id)//если тапнули стену создаем заготовку для копания 
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

    public Hexagon AssignBuildHex(int id)//если тапнули землю создаем заготовку где потом построим стену
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


    private void RemoveFromAllHexList(int id)
    {
        hex = allHex[id];
        buildList.Remove(hex);
        wallList.Remove(hex);
        digList.Remove(hex);
        groundList.Remove(hex);
        spawnList.Remove(hex);
    }


}
