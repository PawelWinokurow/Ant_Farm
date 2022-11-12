using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Jobs;
using PriorityQueue;
using DataStructures.ViliWonka.KDTree;

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
    private List<Worker> busyWorkers = new List<Worker>();
    private List<Worker> freeWorkers = new List<Worker>();
    public List<Worker> AllWorkers = new List<Worker>();

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
        worker.KillMob = () => DestroyWorker(worker);
        freeWorkers.Add(worker);
        AllWorkers.Add(worker);

    }

    private void DestroyWorker(Worker worker)
    {
        busyWorkers.Remove(worker);
        freeWorkers.Remove(worker);
        worker.Job?.Cancel();
        worker.SetState(new DeadState(worker));
    }

    private bool SomeJobLeft()
    {
        return unassignedJobsQueue.Count != 0 && freeWorkers.Count != 0;
    }

    private void AssignWork()
    {

        var freeWorkersClone = new List<Worker>(freeWorkers);
        var job = unassignedJobsQueue[0];

        KDTree workerPositionsTree = new KDTree(freeWorkersClone.Select(worker => worker.Position).ToArray());
        KDQuery query = new KDQuery();
        List<int> queryResults = new List<int>();
        query.KNearest(workerPositionsTree, job.Destination, freeWorkersClone.Count, queryResults);
        foreach (int i in queryResults)
        {
            var path = Pathfinder.FindPath(freeWorkersClone[i].Position, job.Destination, true);
            if (path != null)
            {
                job.Mob = freeWorkersClone[i];
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
        freeWorkers.Remove(freeWorker);
        busyWorkers.Add(freeWorker);
    }
    private void MoveBusyMobToFreeMobs(Worker busyWorker)
    {
        busyWorkers.Remove(busyWorker);
        freeWorkers.Add(busyWorker);
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
