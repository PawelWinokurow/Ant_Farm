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
    private List<Job> notStartedJobsQueue = new List<Job>();
    private List<Job> startedJobsQueue = new List<Job>();
    private List<JobMobDistance> distancesQueue = new List<JobMobDistance>();

    private Graph pathGraph;
    private PathFinder pathFinder;
    private List<Mob> mobs = new List<Mob>();

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            DistributeJobs();
        }
    }

    public void SetGraph(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
        pathFinder = new PathFinder(pathGraph);
    }
    public void AddMob(Mob mob)
    {
        mobs.Add(mob);
    }

    private bool SomeJobLeft()
    {
        return notStartedJobsQueue.Count != 0 && mobs.Count != 0;
    }


    private void DistributeJobs()
    {
        if (SomeJobLeft())
        {
            AssignWork();
        }
        AssignIdle();
    }

    void Update()
    {
        if (distancesQueue.Count != 0)
        {
            SetJobToWorker(distancesQueue[0]);
            distancesQueue.RemoveAt(0);
        }
    }

    private void AssignWork()
    {
        foreach (var mob in mobs)
        {
            JobMobDistance minDistance = null;
            for (int i = notStartedJobsQueue.Count - 1; i >= 0; i--)
            {
                var job = notStartedJobsQueue[i];
                if (job.IsAssigned) continue;
                if (mob.HasJob) continue;
                Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination, true);
                if (path != null && (minDistance == null || path.OverallDistance < minDistance.Path.OverallDistance))
                {
                    minDistance = new JobMobDistance(job, mob, path);
                }
            }
            if (minDistance != null)
            {
                distancesQueue.Add(minDistance);
                moveJobToStartedJobs(minDistance.Job);
            }
        }
    }

    private void moveJobToStartedJobs(Job notStartedJob)
    {
        notStartedJobsQueue.Remove(notStartedJob);
        startedJobsQueue.Add(notStartedJob);
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
        return notStartedJobsQueue.Any(item => item.Id == job.Id);
    }

    public void AssignJob(Job job)
    {
        if (!IsJobAlreadyCreated(job) && !IsJobInQueue(job))
        {
            notStartedJobsQueue.Add(job);
            jobMap.Add(job.Id, job);
        }
    }

    public void RemoveJob(Job job)
    {
        notStartedJobsQueue.Remove(job);
        jobMap.Remove(job.Id);
    }
}
