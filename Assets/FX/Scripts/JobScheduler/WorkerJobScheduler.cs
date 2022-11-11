using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Jobs;
using PriorityQueue;

class JobWorker
{
    public Job Job;
    public Worker Worker;

    public JobWorker(Job job, Worker worker)
    {
        Job = job;
        Worker = worker;
    }
}

public class WorkerJobScheduler : MonoBehaviour
{
    private Dictionary<string, Job> jobMap = new Dictionary<string, Job>();
    private List<Job> unassignedJobsQueue = new List<Job>();
    private List<Job> assignedJobsQueue = new List<Job>();
    private Graph pathGraph;
    private SurfaceOperations SurfaceOperations;
    public Pathfinder Pathfinder { get; set; }
    private List<Worker> busyMobs = new List<Worker>();
    private List<Worker> freeMobs = new List<Worker>();
    public List<Worker> AllMobs = new List<Worker>();

    void Update()
    {
        while (assignedJobsQueue.Count != 0)
        {
            var job = assignedJobsQueue[0];
            assignedJobsQueue.Remove(job);
            MoveFreeMobToBusyMobs((Worker)job.Mob);
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
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void SetSurfaceOperations(SurfaceOperations setSurfaceOperations)
    {
        this.SurfaceOperations = setSurfaceOperations;
    }
    public void AddWorker(Worker worker)
    {
        worker.SurfaceOperations = SurfaceOperations;
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
        var freeMobsClone = new List<Worker>(freeMobs);
        var job = unassignedJobsQueue[0];
        var parallelJob = ParallelComputations.CreateParallelDistancesJob(freeMobsClone.Select(mod => mod.Position).ToList(), job.Destination);
        JobHandle handle = parallelJob.Schedule(freeMobsClone.Count, 4);
        handle.Complete();

        PriorityQueue<JobWorker, float> minDistances = new PriorityQueue<JobWorker, float>(0);

        for (var i = 0; i < parallelJob.Result.Length; i++)
        {
            minDistances.Enqueue(new JobWorker(job, freeMobsClone[i]), parallelJob.Result[i]);
        }
        ParallelComputations.FreeDistancesJobMemory(parallelJob);
        while (minDistances.Count != 0)
        {
            var minDistance = minDistances.Dequeue();
            var path = Pathfinder.FindPath(minDistance.Worker.Position, minDistance.Job.Destination, true);
            if (path != null)
            {
                job.Mob = minDistance.Worker;
                job.Mob.Job = job;
                job.Path = path; ;
                MoveUnassignedJobToAssignedJobs(job);
                return;
            }
        }
        unassignedJobsQueue.Remove(job);
        unassignedJobsQueue.Add(job);
    }

    private void SetJobToWorker(Job job)
    {
        if (job.Type == JobType.CARRYING) SetCarryingJob((CarrierJob)job);
        else if (job.Type == JobType.FILL || job.Type == JobType.DIG) SetWorkerJob((WorkerJob)job);
    }

    private void SetCarryingJob(CarrierJob job)
    {
        var worker = (Worker)job.Mob;
        var path = job.Path;

        job.StorageHexagon = SurfaceOperations.NearestBaseHexagon();
        job.CollectingHexagon = (CollectingHexagon)(job.Hex.Child);
        job.CollectingHexagon.AssignWorker(worker);

        job.Cancel = () =>
        {
            CancelJob(job);
        };

        worker.SetPath(path);
        worker.SetRunAnimation();
        worker.SetState(new GoToState(worker));
    }
    private void SetWorkerJob(WorkerJob job)
    {
        var worker = (Worker)job.Mob;
        var path = job.Path;

        job.Cancel = () =>
        {
            CancelJob(job);
        };

        worker.SetRunAnimation();
        worker.SetState(new GoToState(worker));
        worker.SetPath(path);
    }

    public bool IsJobAlreadyCreated(Job job)
    {
        return IsJobAlreadyCreated(job.Id);
    }
    public bool IsJobAlreadyCreated(string jobId)
    {
        return jobMap.ContainsKey(jobId);
    }
    public bool IsJobUnassigned(Job job)
    {
        return IsJobUnassigned(job.Id);
    }
    public bool IsJobUnassigned(string jobId)
    {
        return unassignedJobsQueue.Any(item => item.Id == jobId);
    }
    public bool IsJobAssigned(Job job)
    {
        return IsJobAssigned(job.Id);
    }
    public bool IsJobAssigned(string jobId)
    {
        return assignedJobsQueue.Any(item => item.Id == jobId);
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
            MoveBusyMobToFreeMobs((Worker)job.Mob);
            job.Mob.SetState(new IdleState((Worker)job.Mob));
            job.Mob.Path = null;
        }
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
