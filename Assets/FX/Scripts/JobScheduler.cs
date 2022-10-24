using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

struct JobAntDistance
{

    public float Distance;
    public Job Job;
    public IAnt Ant;
    public NavMeshPath Path;
    public JobAntDistance(Job job, IAnt ant)
    {
        Path = new NavMeshPath();
        ant.Agent.CalculatePath(job.Destination, Path);
        Distance = Utils.CalculateDistanceFromPoints(Path.corners);
        Job = job;
        Ant = ant;
    }
}

public class JobScheduler : MonoBehaviour
{

    // private List<Job> jobs = new List<Job>();
    private Dictionary<int, Job> jobsMap = new Dictionary<int, Job>();

    private List<IAnt> ants = new List<IAnt>();

    private GameManager gm;

    private IEnumerator coroutine;

    private bool isRunning = false;


    public void AddAnt(IAnt ant)
    {
        ants.Add(ant);
    }

    public void AddAnts(List<IAnt> ants)
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
        List<JobAntDistance> distances = new List<JobAntDistance>();
        foreach (var job in jobsMap.Values)
        {
            foreach (var ant in ants)
            {
                distances.Add(new JobAntDistance(job, ant));
            }
        }
        distances = distances.OrderBy(dist => dist.Distance).ToList();
        distances.ForEach(dist =>
        {
            if (SomeJobLeft())
            {
                RemoveJob(dist.Job);
                dist.Ant.SetPath(dist.Path);
                dist.Ant.SetJob(dist.Job);
            }
        });
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
