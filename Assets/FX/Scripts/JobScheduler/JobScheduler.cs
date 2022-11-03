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

    private Dictionary<string, Job> jobMap = new Dictionary<string, Job>();
    private List<Job> unassignedJobsQueue = new List<Job>();
    private List<Job> assignedJobsQueue = new List<Job>();
    private List<JobMobDistance> distancesQueue = new List<JobMobDistance>();



    private Graph pathGraph;
    private Surface surface;
    private PathFinder pathFinder;
    private List<Mob> busyMobs = new List<Mob>();
    private List<Mob> freeMobs = new List<Mob>();

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            DistributeJobs();
        }
    }

    public void SetGraph(Graph pathGraph)
    {
        this.pathGraph = pathGraph;
        pathFinder = new PathFinder(pathGraph);
    }
    public void SetSurface(Surface surface)
    {
        this.surface = surface;
    }
    public void AddMob(Mob mob)
    {
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
        AssignIdle();
    }

    void Update()
    {
        // if (distancesQueue.Count != 0)
        // {
        // var distance = distancesQueue[0];
        // SetJobToWorker(distance);
        // MoveFreeMobToBusyMobs(distance.Mob);
        // distancesQueue.RemoveAt(0);
        // }
    }

    private void AssignWork()
    {

        var watch = System.Diagnostics.Stopwatch.StartNew();
        var job = unassignedJobsQueue[0];
        var parallelJob = CreateParallelPathFinderJob(freeMobs.Select(mod => mod.CurrentPosition).ToList(), job.Destination);
        JobHandle handle = parallelJob.Schedule(freeMobs.Count, 1);
        handle.Complete();
        PriorityQueue<JobMobDistance, float> minPaths = new PriorityQueue<JobMobDistance, float>(0);
        for (var i = 0; i < parallelJob.Result.Length; i++)
        {
            var path = ManagedObjectWorld.Get(parallelJob.Result[i]);
            minPaths.Enqueue(new JobMobDistance(job, freeMobs[i], path), path.OverallDistance);
        }
        var minPath = minPaths.Dequeue();
        if (minPath != null)
        {

            MoveUnassignedJobToAssignedJobs(minPath.Job);
            MoveFreeMobToBusyMobs(minPath.Mob);
            SetJobToWorker(minPath);
        }
        FreePathFinderJobMemory(parallelJob);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log(elapsedMs);
    }

    private PathFinderJob CreateParallelPathFinderJob(List<Vector3> fromDestinations, Vector3 destination)
    {
        var length = fromDestinations.Count;
        ManagedObjectWorld.Init();
        ManagedObjectRef<PathFinder> pathFinderRef = ManagedObjectWorld.Add(pathFinder);
        PathFinderJob job = new PathFinderJob() { PathFinder = pathFinderRef };
        NativeArray<Vector3> from = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<Vector3> to = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<ManagedObjectRef<Path>> result = new NativeArray<ManagedObjectRef<Path>>(length, Allocator.TempJob);
        for (int i = 0; i < length; i++)
        {
            from[i] = fromDestinations[i];
            to[i] = destination;
        }
        job.From = from;
        job.To = to;
        job.Result = result;
        return job;
    }

    private void FreePathFinderJobMemory(PathFinderJob job)
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

    private void SetJobToWorker(JobMobDistance distance)
    {
        var mob = distance.Mob;
        var job = distance.Job;
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
        Path path = pathFinder.FindPath(mob.CurrentPosition, job.Destination, true);
        mob.SetPath(distance.Path);
    }


    private void AssignIdle()
    {
        freeMobs.ForEach(mob =>
        {
            if (mob.CurrentState.Type == STATE.IDLE)
            {
                if (!mob.HasPath)
                {
                    mob.SetPath(pathFinder.RandomWalk(mob.CurrentPosition, mob.InitialPosition, 5));
                }
            }
        });
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
