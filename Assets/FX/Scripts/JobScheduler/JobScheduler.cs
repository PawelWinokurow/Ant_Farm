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
    private List<Job> unassignedJobsQueue = new List<Job>();
    private List<Job> assignedJobsQueue = new List<Job>();
    private List<JobMobDistance> distancesQueue = new List<JobMobDistance>();



    private Graph pathGraph;
    private Surface surface;
    private PathFinder pathFinder;
    private List<Mob> busyMobs = new List<Mob>();
    private List<Mob> freeMobs = new List<Mob>();

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
    public void SetSurface(Surface surface)
    {
        this.surface = surface;
    }
    public void AddMob(Mob mob)
    {
        freeMobs.Add(mob);
    }

    private bool SomeJobLeft()
    {
        return unassignedJobsQueue.Count != 0 && freeMobs.Count != 0;
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
            var distance = distancesQueue[0];
            SetJobToWorker(distance);
            MoveFreeMobToBusyMobs(distance.Mob);
            distancesQueue.RemoveAt(0);
        }
    }

    private void AssignWork()
    {
        foreach (var mob in freeMobs)
        {
            JobMobDistance minDistance = null;
            for (int i = unassignedJobsQueue.Count - 1; i >= 0; i--)
            {
                var job = unassignedJobsQueue[i];
                Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination, true);
                if (path != null && (minDistance == null || path.OverallDistance < minDistance.Path.OverallDistance))
                {
                    minDistance = new JobMobDistance(job, mob, path);
                }
            }
            if (minDistance != null)
            {
                distancesQueue.Add(minDistance);
                MoveUnassignedJobToAssignedJobs(minDistance.Job);
            }
        }
    }

    private void MoveFreeMobToBusyMobs(Mob freeMob)
    {
        freeMobs.Remove(freeMob);
        busyMobs.Add(freeMob);
    }
    private void MoveBusyMobToFreeMobs(Mob busyMob)
    {
        busyMobs.Remove(busyMob);
        freeMobs.Add(busyMob);
    }

    private void MoveUnassignedJobToAssignedJobs(Job unassignedJob)
    {
        unassignedJobsQueue.Remove(unassignedJob);
        assignedJobsQueue.Add(unassignedJob);
    }
    private void MoveAssignedJobToUnssignedJobs(Job assignedJob)
    {
        assignedJobsQueue.Remove(assignedJob);
        unassignedJobsQueue.Add(assignedJob);
    }

    private void SetJobToWorker(JobMobDistance distance)
    {
        var mob = distance.Mob;
        var job = distance.Job;
        mob.Job = job;
        job.Mob = mob;
        job.Execute = () =>
        {
            surface.StartHexJobExecution(((DiggerJob)job).Hex, mob);
        };
        job.Remove = () =>
        {
            CancelJob(job);
        };
        mob.SetState(new GoToState((Digger)mob));
        Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination, true);
        mob.SetPath(distance.Path);
    }


    private void AssignIdle()
    {
        freeMobs.ForEach(mob =>
        {
            if (mob.CurrentState.Type == STATE.IDLE)
            {
                if (!mob.HasPath)
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
    public bool IsJobAlreadyCreated(string jobId)
    {
        return jobMap.ContainsKey(jobId);
    }

    public bool IsJobUnassigned(Job job)
    {
        return unassignedJobsQueue.Any(item => item.Id == job.Id);
    }
    public bool IsJobAssigned(Job job)
    {
        return assignedJobsQueue.Any(item => item.Id == job.Id);
    }

    public void AssignJob(Job job)
    {
        if (!IsJobAlreadyCreated(job) && !IsJobUnassigned(job))
        {
            jobMap.Add(job.Id, job);
            unassignedJobsQueue.Add(job);
        }
    }

    public void CancelJob(string jobId)
    {
        var job = jobMap[jobId];
        var mob = job.Mob;
        Remove(job);
        if (mob != null)
        {
            MoveBusyMobToFreeMobs(mob);
            job.Mob.SetState(new IdleState((Digger)mob));
        }
    }
    public void CancelJob(Job job)
    {
        var mob = job.Mob;
        Remove(job);
        MoveBusyMobToFreeMobs(mob);
        job.Mob.SetState(new IdleState((Digger)mob));
    }

    public void Remove(Job job)
    {
        assignedJobsQueue.Remove(job);
        unassignedJobsQueue.Remove(job);
        jobMap.Remove(job.Id);
    }

}
