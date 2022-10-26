using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public Surface surface;


    public List<Mob> antsList = new List<Mob>();
    public GameObject antBuilderPrefab;

    public JobScheduler JobScheduler;
    private static GameManager instance;

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
        surface.Init();
        // AntJobScheduler.AddAnts(antsList);
    }

}
