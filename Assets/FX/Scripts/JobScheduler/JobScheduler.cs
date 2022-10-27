using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

class JobDiggerDistance
{
    public float Distance;
    public Job Job;
    public Digger Digger;

    public JobDiggerDistance(Job job, Digger digger, float distance)
    {
        Job = job;
        Digger = digger;
        Distance = distance;
    }

}

public class JobScheduler : MonoBehaviour
{

    private Dictionary<int, Job> jobsMap = new Dictionary<int, Job>();

    private List<Digger> diggers = new List<Digger>();

    private GameManager gm;

    void Start()
    {
        gm = GameManager.GetInstance();
    }

    public void AddDigger(Digger ant)
    {
        diggers.Add(ant);
    }

    public void AddDiggers(List<Digger> diggers)
    {
        this.diggers.AddRange(diggers);
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
        List<JobDiggerDistance> distances = new List<JobDiggerDistance>();
        foreach (var job in jobsMap.Values)
        {
            foreach (var digger in diggers)
            {
                // var path = gm.Surface.PathGraph.FindPath(digger.CurrentPosition, job.Destination);
                // if (path != null)
                // {
                //     distances.Add(new JobDiggerDistance(job, digger, path.OverallDistance));

                // }

                distances.Add(new JobDiggerDistance(job, digger, Vector3.Distance(digger.CurrentPosition, job.Destination)));

            }
        }
        distances = distances.OrderBy(dist => dist.Distance).ToList();
        distances.ForEach(dist =>
        {
            if (SomeJobLeft())
            {
                if (dist.Digger.WayPoints.Count == 0)
                {
                    RemoveJob(dist.Job);
                    dist.Digger.SetJob(dist.Job);
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
