using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public Surface Surface;
    public BuildWallsTest BuildWallsTest;
    public List<Mob> antsList = new List<Mob>();
    public JobScheduler JobScheduler;
    private static GameManager instance;

    public GameObject DiggerPrefab;

    private GameManager()
    {
        instance = this;
    }

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }


    void Awake()
    {
        Surface.Init();
        BuildWallsTest.Test();
        InstantiateTestMobs();
    }

    void InstantiateTestMobs()
    {
        var gameObjects = new List<GameObject>();
        gameObjects.Add(Instantiate(DiggerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        gameObjects.Add(Instantiate(DiggerPrefab, new Vector3(30, 0, 0), Quaternion.identity));
        gameObjects.Add(Instantiate(DiggerPrefab, new Vector3(0, 0, 30), Quaternion.identity));
        JobScheduler.AddDiggers(gameObjects.Select(obj => obj.GetComponent<Digger>()).ToList());
    }

}
