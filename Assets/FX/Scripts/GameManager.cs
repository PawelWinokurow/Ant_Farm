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
        PathGraph = null;
        if (StoreService.DoesGraphExist())
        {
            PathGraph = StoreService.LoadGraph();
            Surface.Init(PathGraph, false);
        }
        else
        {
            PathGraph = new Graph();
            Surface.Init(PathGraph, true);
            StoreService.SaveGraph(PathGraph);
        }
        JobScheduler.SetGraph(PathGraph);
        BuildWallsTest.Init(Surface);
    }
    void Start()
    {
        InstantiateTestMobs();
    }
    void InstantiateTestMobs()
    {
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(30, 0, 30), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(50, 0, 0), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(0, 0, 50), Quaternion.identity));
    }

    public void ProcessTap(Vector3 position)
    {
        Hexagon hex = Surface.PositionToHex(position);
        Surface.AddIcon(hex);

        if (hex.IsEmpty)
        {
            JobScheduler.AssignJob(new DigJob(hex, hex.transform.position, AssignmentFactory.CreateFillAssignment(hex)));
        }
        else if (hex.IsSoil)
        {
            JobScheduler.AssignJob(new DigJob(hex, hex.transform.position, AssignmentFactory.CreateDigAssignment(hex)));
        }


    }



}

