using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class JobScheduler : MonoBehaviour
{

    private Dictionary<int, Job> jobsMap = new Dictionary<int, Job>();

    private List<Mob> ants = new List<Mob>();

    // private GameManager gm;


    public void AddAnt(Mob ant)
    {
        ants.Add(ant);
    }

    public void AddAnts(List<Mob> ants)
    {
        this.ants.AddRange(ants);
    }

    public void Update()
    {
        if (SomeJobLeft())
        {
            AssignJobs();
        }
    }

    private bool SomeJobLeft()
    {
        return jobsMap.Count != 0;
    }


    private void AssignJobs()
    {
        // List<JobAntDistance> distances = new List<JobAntDistance>();
        // foreach (var job in jobsMap.Values)
        // {
        //     foreach (var ant in ants)
        //     {
        //         distances.Add(new JobAntDistance(job, ant));
        //     }
        // }
        // distances = distances.OrderBy(dist => dist.Distance).ToList();
        // distances.ForEach(dist =>
        // {
        //     if (SomeJobLeft())
        //     {
        //         RemoveJob(dist.Job);
        //         dist.Ant.SetPath(dist.Path);
        //         dist.Ant.SetJob(dist.Job);
        //     }
        // });
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
