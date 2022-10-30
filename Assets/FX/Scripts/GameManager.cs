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
        BuildWallsTest.Init(Surface);
        InstantiateTestMobs();
    }

    void InstantiateTestMobs()
    {
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(50, 0, 0), Quaternion.identity));
        JobScheduler.AddMob(Instantiate(DiggerPrefab, new Vector3(0, 0, 50), Quaternion.identity));
    }

    public void ProcessTap(Vector3 position)
    {
        Hexagon hex = Surface.PositionToHex(position);
        if (!JobScheduler.IsJobAlreadyCreated(hex.id))
        {
            Debug.Log(hex.IsDigabble());
            Debug.Log(hex.IsBuildable());
            if (hex.IsDigabble())
            {
                JobScheduler.AssignJob(new DigJob(hex, hex.transform.position, AssignmentFactory.CreateDigAssignment(hex)));
            }
            else if (hex.IsBuildable())
            {
                JobScheduler.AssignJob(new DigJob(hex, hex.transform.position, AssignmentFactory.CreateBuryAssignment(hex)));
            }
        }
    }



}

