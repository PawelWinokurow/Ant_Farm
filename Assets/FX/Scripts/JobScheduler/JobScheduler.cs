using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using PriorityQueue;

class JobMobDistance
{
    public Job Job;
    public Mob Mob;
    public float Distance;

    public JobMobDistance(Job job, Mob mob, float distance)
    {
        Job = job;
        Mob = mob;
        Distance = distance;
    }

}

public class JobScheduler : MonoBehaviour
{
    private Dictionary<string, Job> jobMap = new Dictionary<string, Job>();
    private List<Job> unassignedJobsQueue = new List<Job>();
    private List<Job> assignedJobsQueue = new List<Job>();
    private Graph pathGraph;
    private Surface surface;
    public Pathfinder Pathfinder { get; set; }
    private List<Mob> busyMobs = new List<Mob>();
    private List<Mob> freeMobs = new List<Mob>();

    void Update()
    {
        if (assignedJobsQueue.Count != 0)
        {
            var job = assignedJobsQueue[0];
            assignedJobsQueue.RemoveAt(0);
            MoveFreeMobToBusyMobs(job.Mob);
            SetJobToWorker(job);
        }
    }

    public void StartJobScheuler()
    {
        StartCoroutine(DistributeJobsCoroutine());
    }
    IEnumerator DistributeJobsCoroutine()
    {
        while (true)
        {
            if (SomeJobLeft())
            {
                AssignWork();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetSurface(Surface surface)
    {
        this.surface = surface;
    }
    public void AddMob(Mob mob)
    {
        mob.Pathfinder = Pathfinder;
        freeMobs.Add(mob);
    }

    private bool SomeJobLeft()
    {
        return unassignedJobsQueue.Count != 0 && freeMobs.Count != 0;
    }

    private void AssignWork()
    {
        var job = unassignedJobsQueue[0];
        var parallelJob = ParallelComputations.CreateParallelDistancesJob(freeMobs.Select(mod => mod.CurrentPosition).ToList(), job.Destination);
        JobHandle handle = parallelJob.Schedule(freeMobs.Count, 1);
        handle.Complete();
        PriorityQueue<JobMobDistance, float> minDistances = new PriorityQueue<JobMobDistance, float>(0);

        for (var i = 0; i < parallelJob.Result.Length; i++)
        {
            minDistances.Enqueue(new JobMobDistance(job, freeMobs[i], parallelJob.Result[i]), parallelJob.Result[i]);
        }
        Path path = null;
        while (minDistances.Count != 0)
        {
            var minDistance = minDistances.Dequeue();
            path = Pathfinder.FindPath(minDistance.Mob.CurrentPosition, minDistance.Job.Destination, true);
            if (path != null)
            {
                job.Mob = minDistance.Mob;
                job.Path = path; ;
                MoveUnassignedJobToAssignedJobs(job);
                break;
            }
        }
        ParallelComputations.FreeDistancesJobMemory(parallelJob);
    }



    private void SetJobToWorker(Job job)
    {
        var mob = job.Mob;
        mob.Job = job;
        var path = job.Path;
        job.Execute = () =>
        {
            surface.StartHexJobExecution(((DiggerJob)job).Hex, mob);
        };
        job.CancelJob = () =>
        {
            CancelJob(job);
        };
        job.CancelNotCompleteJob = () =>
        {
            CancelNotCompleteJob(job);
        };
        mob.SetState(new GoToState((Digger)mob));
        mob.SetPath(path);
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
        CancelJob(jobMap[jobId]);
    }
    public void CancelJob(Job job)
    {
        Remove(job);
        if (job.Mob != null)
        {
            MoveBusyMobToFreeMobs(job.Mob);
            job.Mob.SetState(new IdleState((Digger)job.Mob));
        }

    }
    public void CancelNotCompleteJob(Job job)
    {
        MoveAssignedJobToUnssignedJobs(job);
    }

    public void Remove(Job job)
    {
        assignedJobsQueue.Remove(job);
        unassignedJobsQueue.Remove(job);
        jobMap.Remove(job.Id);
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

}
