using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public Spawn spawn;
    public Creator creator;
    public Surface surface;
    private Vector3 pos;


    public List<IAnt> antsList = new List<IAnt>();
    public GameObject antBuilderPrefab;
    public GameObject antDiggerPrefab;

    private IEnumerator coroutine;

    public JobScheduler AntJobScheduler;
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
        Screen.orientation = ScreenOrientation.Portrait;

        creator.Init(surface);
        spawn.Init(surface, this);
        AntJobScheduler.AddAnts(antsList);
    }

}
