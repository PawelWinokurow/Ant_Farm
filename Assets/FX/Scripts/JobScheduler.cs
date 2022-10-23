using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class JobScheduler<T>
{

    // private List<Job> jobs = new List<Job>();
    private Dictionary<int, Job> jobsMap = new Dictionary<int, Job>();

    private List<IAnt> ants = new List<IAnt>();

    private static JobScheduler<T> instance;

    private GameManager gm;

    private IEnumerator coroutine;

    private bool isRunning = false;

    private JobScheduler()
    {
        instance = this;
        UpdateInterval();
    }

    public void Start()
    {
        isRunning = true;
    }
    public void False()
    {
        isRunning = false;
    }

    private void UpdateInterval()
    {
        while (isRunning)
        {
            Update();
        }
    }

    public static JobScheduler<T> GetInstance()
    {
        if (instance == null)
        {
            instance = new JobScheduler<T>();
        }
        return instance;
    }

    public void AddAnt(IAnt ant)
    {
        ants.Add(ant);
    }

    public void AddAnts(List<IAnt> ants)
    {
        this.ants.AddRange(ants);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update()
    {
        ants.ForEach(worker => { if (SomeJobLeft() && !worker.HasJob()) { AssignClosestJob(worker); } });
    }

    private bool SomeJobLeft()
    {
        return jobsMap.Count != 0;
    }

    private void AssignClosestJob(IAnt worker)
    {
        double minPathLength = double.MaxValue;
        Job minPathJob = null;
        NavMeshPath minPath = null;
        NavMeshPath navMeshPath = new NavMeshPath();
        foreach (var job in jobsMap.Values)
        {
            worker.Agent.CalculatePath(job.Destination, navMeshPath);
            float pathLength = Utils.CalculateDistanceFromPoints(navMeshPath.corners);
            if (navMeshPath.status == NavMeshPathStatus.PathComplete && pathLength < minPathLength)
            {
                minPathLength = pathLength;
                minPathJob = job;
                minPath = navMeshPath;
            }
        }
        if (minPath != null)
        {
            RemoveJob(minPathJob);
            worker.SetPath(minPath);
            worker.SetJob(minPathJob);
        }
    }

    public bool IsJobAlreadyCreated(int Id)
    {
        return jobsMap.ContainsKey(Id);
    }

    public void AssignJob(Job job)
    {
        jobsMap.Add(job.Id, job);
    }

    public void RemoveJob(Job job)
    {
        jobsMap.Remove(job.Id);
    }
}
