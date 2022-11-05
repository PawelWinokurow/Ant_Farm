using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Jobs;
using PriorityQueue;

class JobMob
{
    public Job Job;
    public Mob Mob;

    public JobMob(Job job, Mob mob)
    {
        Job = job;
        Mob = mob;
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
    public List<Mob> AllMobs = new List<Mob>();

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
            yield return new WaitForSeconds(0.5f);
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
        AllMobs.Add(mob);
    }

    private bool SomeJobLeft()
    {
        return unassignedJobsQueue.Count != 0 && freeMobs.Count != 0;
    }

    private void AssignWork()
    {
        var job = unassignedJobsQueue[0];
        var parallelJob = ParallelComputations.CreateParallelDistancesJob(freeMobs.Select(mod => mod.CurrentPosition).ToList(), job.Destination);
        JobHandle handle = parallelJob.Schedule(freeMobs.Count, 4);
        handle.Complete();

        PriorityQueue<JobMob, float> minDistances = new PriorityQueue<JobMob, float>(0);

        for (var i = 0; i < parallelJob.Result.Length; i++)
        {
            minDistances.Enqueue(new JobMob(job, freeMobs[i]), parallelJob.Result[i]);
        }
        while (minDistances.Count != 0)
        {
            var minDistance = minDistances.Dequeue();
            var path = Pathfinder.FindPath(minDistance.Mob.CurrentPosition, minDistance.Job.Destination, true);
            if (path != null)
            {
                job.Mob = minDistance.Mob;
                job.Mob.Job = job;
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
        var path = job.Path;
        if (job.Type == JobType.CARRYING)
        {
            var carrierJob = (CarrierJob)job;
            carrierJob.Return = () =>
            {
                var swap = carrierJob.Destination;
                carrierJob.Destination = carrierJob.Departure;
                carrierJob.Departure = swap;
                mob.SetPath(Pathfinder.FindPath(mob.CurrentPosition, carrierJob.Destination, true));
                mob.SetState(new GoToState((Worker)mob));
                Debug.Log("return");
            };
        }
        job.Execute = () =>
        {
            surface.StartJobExecution(job.Hex, mob);
        };
        job.CancelJob = () =>
        {
            CancelJob(job);
        };
        job.CancelNotCompleteJob = () =>
        {
            CancelNotCompleteJob(job);
        };
        mob.SetState(new GoToState((Worker)mob));
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
            job.Mob.SetState(new IdleState((Worker)job.Mob));
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
