using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public NavMeshSurface navSurface;
    // public GameObject back;
    public List<Hexagon> digList;
    public List<Hexagon> groundList;
    public List<Hexagon> spawnList;
    public List<Hexagon> wallList;
    public List<Hexagon> buildList;
    public List<Vector3> sideList;
    public Hexagon groundPrefab;
    public Hexagon wallPrefab;
    public Hexagon digPrefab;
    public Hexagon buildPrefab;

    public List<IAnt> antsList;
    public Ant_Digger antPrefab;

    public List<Enemy> enemyList;
    public Enemy enemyPrefab;
    public Spawn spawn;

    void Awake()
    {
        instance = this;
        navSurface = GetComponent<NavMeshSurface>();
        digList = new List<Hexagon>();
        groundList = new List<Hexagon>();
        spawnList = new List<Hexagon>();
        wallList = new List<Hexagon>();
        antsList = new List<IAnt>();
    }

    public void AddSpawn(Hexagon hex)//для зоны спауна 
    {
        Vector3 pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        wallList.Remove(hex);
        hex = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
        hex.isSpawn = true;
        spawnList.Add(hex);//здесь будем спаунить муравьев
        groundList.Add(hex);
        navSurface.BuildNavMesh();
    }

    public void AddGround(Hexagon hex)//при создании земли по краям для спавна врагов
    {
        Vector3 pos = hex.transform.position;
        GameObject.Destroy(hex.gameObject);
        wallList.Remove(hex);
        hex = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
        groundList.Add(hex);
        navSurface.BuildNavMesh();
    }


    public Hexagon TapWall(Hexagon hex)//если тапнули стену создаем заготовку для копания 
    {

        Vector3 pos = hex.transform.position;
        wallList.Remove(hex);
        GameObject.Destroy(hex.gameObject);//удаляем стену

        Hexagon dig = Instantiate(digPrefab, pos, Quaternion.identity, transform);
        digList.Add(dig);
        navSurface.BuildNavMesh();
        return dig;
    }

    public Hexagon TapGround(Hexagon hex)//если тапнули землю создаем заготовку где потом построим стену
    {

        Vector3 pos = hex.transform.position;
        groundList.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        Hexagon build = Instantiate(buildPrefab, pos, Quaternion.identity, transform);
        buildList.Add(build);
        //navSurface.BuildNavMesh();
        return build;
    }


    public void Dig(Hexagon hex)//если выкопали
    {

        Vector3 pos = hex.transform.position;
        digList.Remove(hex);
        GameObject.Destroy(hex.gameObject);
        hex = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
        groundList.Add(hex);
        navSurface.BuildNavMesh();

    }



}
