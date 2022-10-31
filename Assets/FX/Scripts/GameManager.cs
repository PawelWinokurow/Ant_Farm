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
        Surface.Init(PathGraph);
        JobScheduler.SetGraph(PathGraph);
        BuildWallsTest.Init(Surface);
        InstantiateTestMobs();
    }

    void InstantiateTestMobs()
    {
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(30, 0, 30), Quaternion.identity));
        // JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(50, 0, 0), Quaternion.identity));
        // JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(0, 0, 50), Quaternion.identity));
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

