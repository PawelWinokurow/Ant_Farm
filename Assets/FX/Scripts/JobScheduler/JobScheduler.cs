using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Jobs;
using PriorityQueue;

class JobMob
{
    public Job Job;
    public Worker Worker;

    public JobMob(Job job, Worker worker)
    {
        Job = job;
        Worker = worker;
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
    private List<Worker> busyMobs = new List<Worker>();
    private List<Worker> freeMobs = new List<Worker>();
    public List<Worker> AllMobs = new List<Worker>();

    void Update()
    {
        if (assignedJobsQueue.Count != 0)
        {
            var job = assignedJobsQueue[0];
            assignedJobsQueue.RemoveAt(0);
            MoveFreeMobToBusyMobs(job.Worker);
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
    public void AddMob(Worker worker)
    {
        worker.Pathfinder = Pathfinder;
        freeMobs.Add(worker);
        AllMobs.Add(worker);
    }

    private bool SomeJobLeft()
    {
        return unassignedJobsQueue.Count != 0 && freeMobs.Count != 0;
    }

    private void AssignWork()
    {
        var job = unassignedJobsQueue[0];
        var parallelJob = ParallelComputations.CreateParallelDistancesJob(freeMobs.Select(mod => mod.Position).ToList(), job.Destination);
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
            var path = Pathfinder.FindPath(minDistance.Worker.Position, minDistance.Job.Destination, true);
            if (path != null)
            {
                job.Worker = minDistance.Worker;
                job.Worker.Job = job;
                job.Path = path; ;
                MoveUnassignedJobToAssignedJobs(job);
                break;
            }
        }
        ParallelComputations.FreeDistancesJobMemory(parallelJob);
    }

    private void SetJobToWorker(Job job)
    {
        if (job.Type == JobType.CARRYING) SetCarryingJob((CarrierJob)job);
        else if (job.Type == JobType.FILL || job.Type == JobType.DIG) SetWorkerJob((WorkerJob)job);
    }

    private void SetCarryingJob(CarrierJob job)
    {
        var worker = job.Worker;
        var path = job.Path;
        job.Direction = Direction.COLLECTING;
        var collectingHex = (CollectingHexagon)(job.Hex.Child);
        collectingHex.AssignWorker(worker);
        job.Return = () =>
        {
            job.Direction = job.Direction == Direction.STORAGE ? Direction.COLLECTING : Direction.STORAGE;
            job.Destination = job.Direction == Direction.STORAGE ? job.StoragePosition : job.CollectingPointPosition;
            worker.SetPath(Pathfinder.FindPath(worker.Position, job.Destination, true));
            worker.SetState(new GoToState(worker));
        };
        job.CancelJob = () =>
        {
            if (worker.CarryingWeight == 0 || job.Direction == Direction.COLLECTING)
            {
                job.Direction = Direction.CANCELED;
                CancelJob(job);
            }
        };

        job.Execute = () =>
        {
            surface.StartJobExecution(job.Hex, worker);
        };

        job.CancelNotCompleteJob = () =>
        {
            CancelNotCompleteJob(job);
        };
        worker.SetState(new GoToState(worker));
        worker.SetPath(path);
    }
    private void SetWorkerJob(WorkerJob job)
    {
        var mob = job.Worker;
        var path = job.Path;

        {
            job.CancelJob = () =>
            {
                CancelJob(job);
            };

            job.Execute = () =>
            {
                surface.StartJobExecution(job.Hex, mob);
            };

            job.CancelNotCompleteJob = () =>
            {
                CancelNotCompleteJob(job);
            };
            mob.SetState(new GoToState((Worker)mob));
            mob.SetPath(path);
        }
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
        if (job.Worker != null)
        {
            MoveBusyMobToFreeMobs(job.Worker);
            job.Worker.SetState(new IdleState((Worker)job.Worker));
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

    private void MoveFreeMobToBusyMobs(Worker freeWorker)
    {
        freeMobs.Remove(freeWorker);
        busyMobs.Add(freeWorker);
    }
    private void MoveBusyMobToFreeMobs(Worker busyWorker)
    {
        busyMobs.Remove(busyWorker);
        freeMobs.Add(busyWorker);
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
