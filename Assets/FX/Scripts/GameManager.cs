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
    public JobScheduler JobScheduler;

    public Digger DiggerPrefab;

    public Graph PathGraph;

    void Awake()
    {
        PathGraph = new Graph();
        JobScheduler.SetGraph(PathGraph);
        Surface.Init(PathGraph);
        BuildWallsTest.Test();
        InstantiateTestMobs();
    }

    void InstantiateTestMobs()
    {
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(50, 0, 0), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(0, 0, 50), Quaternion.identity));
    }

    public void ProcessHexagon(Vector3 position)
    {
        Hexagon hex = Surface.allHex[Surface.PositionToId(position)];
        if (!JobScheduler.IsJobAlreadyCreated(hex.id))
        {
            hex.AssignDig();
            JobScheduler.AssignJob(new DigJob(hex, position));
        }
    }

}

