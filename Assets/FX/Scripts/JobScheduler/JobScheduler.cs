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
    private Pathfinder pathfinder;
    private List<Mob> busyMobs = new List<Mob>();
    private List<Mob> freeMobs = new List<Mob>();

    public void StartJobScheuler()
    {
        StartCoroutine(DistributeJobsCoroutine());
    }
    IEnumerator DistributeJobsCoroutine()
    {
        while (true)
        {
            DistributeJobs();
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void SetPathfinder(Pathfinder pathfinder)
    {
        this.pathfinder = pathfinder;
        ManagedObjectWorld.Init();
    }

    public void SetSurface(Surface surface)
    {
        this.surface = surface;
    }
    public void AddMob(Mob mob)
    {
        mob.Pathfinder = pathfinder;
        freeMobs.Add(mob);
    }

    private bool SomeJobLeft()
    {
        return unassignedJobsQueue.Count != 0 && freeMobs.Count != 0;
    }


    private void DistributeJobs()
    {
        if (SomeJobLeft())
        {
            AssignWork();
        }
    }

    private void AssignWork()
    {
        // var job = unassignedJobsQueue[0];
        // var parallelJob = CreateParallelPathfinderJob(freeMobs.Select(mod => mod.CurrentPosition).ToList(), job.Destination);
        // JobHandle handle = parallelJob.Schedule(freeMobs.Count, 1);
        // handle.Complete();
        // PriorityQueue<JobMobDistance, float> minPaths = new PriorityQueue<JobMobDistance, float>(0);
        // for (var i = 0; i < parallelJob.Result.Length; i++)
        // {
        //     var path = ManagedObjectWorld.Get(parallelJob.Result[i]);
        //     minPaths.Enqueue(new JobMobDistance(job, freeMobs[i], path), path.Length);
        // }
        // var minPath = minPaths.Dequeue();
        // if (minPath != null)
        // {
        //     MoveUnassignedJobToAssignedJobs(minPath.Job);
        //     MoveFreeMobToBusyMobs(minPath.Mob);
        //     SetJobToWorker(minPath);
        // }
        // FreePathfinderJobMemory(parallelJob);

        var job = unassignedJobsQueue[0];
        var parallelJob = CreateParallelDistancesJob(freeMobs.Select(mod => mod.CurrentPosition).ToList(), job.Destination);
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
            path = pathfinder.FindPath(minDistance.Mob.CurrentPosition, minDistance.Job.Destination, true);
            if (path != null)
            {
                MoveUnassignedJobToAssignedJobs(minDistance.Job);
                MoveFreeMobToBusyMobs(minDistance.Mob);
                SetJobToWorker(minDistance.Mob, minDistance.Job, path);
                break;
            }
        }

        parallelJob.From.Dispose();
        parallelJob.To.Dispose();
        parallelJob.Result.Dispose();
    }

    private PathfinderJob CreateParallelPathfinderJob(List<Vector3> fromPositions, Vector3 destinationPosition)
    {
        var length = fromPositions.Count;
        ManagedObjectWorld.Clear();
        NativeArray<Vector3> from = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<Vector3> to = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<ManagedObjectRef<Path>> result = new NativeArray<ManagedObjectRef<Path>>(length, Allocator.TempJob);

        for (int i = 0; i < length; i++)
        {
            from[i] = fromPositions[i];
            to[i] = destinationPosition;
        }
        return new PathfinderJob() { Pathfinder = ManagedObjectWorld.Add(pathfinder), From = from, To = to, Result = result };
    }
    private DistancesJob CreateParallelDistancesJob(List<Vector3> fromPositions, Vector3 destinationPosition)
    {
        var length = fromPositions.Count;
        NativeArray<Vector3> from = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<Vector3> to = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<float> result = new NativeArray<float>(length, Allocator.TempJob);

        for (int i = 0; i < length; i++)
        {
            from[i] = fromPositions[i];
            to[i] = destinationPosition;
        }
        return new DistancesJob() { From = from, To = to, Result = result };
    }

    public struct DistancesJob : IJobParallelFor
    {
        public NativeArray<Vector3> From;
        public NativeArray<Vector3> To;
        public NativeArray<float> Result;

        public void Execute(int i)
        {
            Result[i] = Vector3.Distance(From[i], To[i]);
        }
    }

    private void FreePathfinderJobMemory(PathfinderJob job)
    {
        job.From.Dispose();
        job.To.Dispose();
        job.Result.Dispose();
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

    private void SetJobToWorker(Mob mob, Job job, Path path)
    {
        mob.Job = job;
        job.Mob = mob;
        job.Execute = () =>
        {
            surface.StartHexJobExecution(((DiggerJob)job).Hex, mob);
        };
        job.Remove = () =>
        {
            CancelJob(job);
        };
        job.NotComplete = () =>
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

}
