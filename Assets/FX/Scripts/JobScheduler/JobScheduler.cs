using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

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

    private Dictionary<string, Job> jobsMap = new Dictionary<string, Job>();

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
        return jobsMap.Count != 0;
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
        List<JobMobDistance> distances = new List<JobMobDistance>();
        foreach (var job in jobsMap.Values)
        {
            foreach (var mob in mobs)
            {
                Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination, true);
                if (path != null)
                {
                    distances.Add(new JobMobDistance(job, mob, path));
                }

            }
        }
        distances = distances.OrderBy(distance => distance.Path.OverallDistance).ToList();
        distances.ForEach(distance =>
        {
            if (SomeJobLeft())
            {
                if (distance.Mob.CurrentState.Type == STATE.IDLE && !distance.Job.IsAssigned)
                {
                    distance.Job.IsAssigned = true;
                    var mob = distance.Mob;
                    var job = distance.Job;
                    mob.Job = job;
                    mob.Job.RemoveJob = () =>
                    {
                        mob.SetState(new IdleState((Digger)mob));
                        RemoveJob(job);
                    };
                    mob.SetState(new GoToState((Digger)mob));
                    mob.SetPath(distance.Path);
                }
            }
        });
    }


    private void AssignIdle()
    {
        mobs.ForEach(mob =>
        {
            if (mob.CurrentState.Type == STATE.IDLE)
            {
                if (!mob.HasPath)
                {
                    mob.SetPath(pathFinder.RandomWalk(mob.CurrentPosition, mob.InitialPosition, 10));
                }
                else if (mob.WayPoints?.Count == 0)
                {
                    var newPath = pathFinder.RandomWalk(mob.WayPoints[mob.WayPoints.Count - 1], mob.InitialPosition, 10);
                    mob.Path.WayPoints.AddRange(newPath.WayPoints);
                    mob.Path.OverallDistance += newPath.OverallDistance;
                }

            }
        });
    }

    public bool IsJobAlreadyCreated(string Id)
    {
        return jobsMap.ContainsKey(Id);
    }

    public void AssignJob(Job job)
    {
        if (!IsJobAlreadyCreated(job.Id))
        {
            jobsMap.Add(job.Id, job);
        }
        else
        {
            jobsMap[job.Id].RemoveJob();
        }
    }

    public void RemoveJob(Job job)
    {
        jobsMap.Remove(job.Id);
    }
}
