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

    private Dictionary<int, Job> jobsMap = new Dictionary<int, Job>();

    private Graph pathGraph;
    private PathFinder pathFinder;


    private List<Mob> mobs = new List<Mob>();

    public void SetGraph(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
        pathFinder = new PathFinder(pathGraph);
    }
    public void AddMob(Mob ant)
    {
        mobs.Add(ant);
    }

    public void Update()
    {
        if (SomeJobLeft())
        {
            DistributeJobs();
        }
    }

    private bool SomeJobLeft()
    {
        return jobsMap.Count != 0;
    }


    private void DistributeJobs()
    {
        List<JobMobDistance> distances = new List<JobMobDistance>();
        foreach (var job in jobsMap.Values)
        {
            foreach (var mob in mobs)
            {
                Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination);
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
                if (distance.Mob.CurrentState.Type == STATE.IDLE)
                {
                    RemoveJob(distance.Job);
                    distance.Mob.Job = distance.Job;
                    distance.Mob.Path = distance.Path;
                    //TODO make generic
                    distance.Mob.SetState(new GoToState((Digger)distance.Mob));
                }
            }
        });
    }

    public bool IsJobAlreadyCreated(int Id)
    {
        return jobsMap.ContainsKey(Id);
    }

    public void AssignJob(Job job)
    {
        if (!IsJobAlreadyCreated(job.Id))
        {
            jobsMap.Add(job.Id, job);
        }
    }

    public void RemoveJob(Job job)
    {
        jobsMap.Remove(job.Id);
    }
}
