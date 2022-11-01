using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using PriorityQueue;

class JobMobDistance
{
    public Job Job;
    public Mob Mob;
    public Path Path;

    public JobMobDistance(Job job, Mob mob, Path path)
    {
        Job = job;
        Mob = mob;
        Path = path;
    }

}

public class JobScheduler : MonoBehaviour
{

    private Dictionary<string, Job> jobMap = new Dictionary<string, Job>();
    private List<Job> jobQueue = new List<Job>();

    private Graph pathGraph;
    private PathFinder pathFinder;
    private List<Mob> mobs = new List<Mob>();

    public void SetGraph(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
        pathFinder = new PathFinder(pathGraph);
    }
    public void AddMob(Mob mob)
    {
        mobs.Add(mob);
    }

    public void Update()
    {
        DistributeJobs();
    }

    private bool SomeJobLeft()
    {
        return jobQueue.Count != 0 && mobs.Count != 0;
    }


    private void DistributeJobs()
    {
        if (SomeJobLeft())
        {
            AssignWork();
        }
        AssignIdle();
    }

    private void AssignWork()
    {

        foreach (var job in jobQueue)
        {
            if (job.IsAssigned) continue;
            JobMobDistance minDistance = null;
            foreach (var mob in mobs)
            {
                if (mob.HasJob) continue;
                Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination, true);
                if (path != null && (minDistance == null || path.OverallDistance < minDistance.Path.OverallDistance))
                {
                    minDistance = new JobMobDistance(job, mob, path);
                }
            }
            if (minDistance != null)
            {
                SetJobToWorker(minDistance);
            }
        }
    }

    private void SetJobToWorker(JobMobDistance distance)
    {
        var mob = distance.Mob;
        var job = distance.Job;
        distance.Job.IsAssigned = true;
        mob.Job = job;
        mob.Job.RemoveJob = () =>
        {
            mob.SetState(new IdleState((Digger)mob));
            RemoveJob(job);
        };
        mob.SetState(new GoToState((Digger)mob));
        Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination, true);
        mob.SetPath(distance.Path);
    }


    private void AssignIdle()
    {
        mobs.ForEach(mob =>
        {
            if (mob.CurrentState.Type == STATE.IDLE)
            {
                if (!mob.HasPath || mob.WayPoints?.Count == 0)
                {
                    mob.SetPath(pathFinder.RandomWalk(mob.CurrentPosition, mob.InitialPosition, 5));
                }
            }
        });
    }

    public bool IsJobAlreadyCreated(Job job)
    {
        return jobMap.ContainsKey(job.Id);
    }

    public bool IsJobInQueue(Job job)
    {
        return jobQueue.Any(item => item.Id == job.Id);
    }

    public void AssignJob(Job job)
    {
        if (!IsJobAlreadyCreated(job) && !IsJobInQueue(job))
        {
            jobQueue.Add(job);
            jobMap.Add(job.Id, job);
        }
    }

    public void RemoveJob(Job job)
    {
        jobMap.Remove(job.Id);
        jobQueue.Remove(job);
    }
}
