using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Jobs;
using PriorityQueue;

class JobEnemy
{
    public Job Job;
    public Enemy Enemy;

    public JobEnemy(Job job, Enemy enemy)
    {
        Job = job;
        Enemy = enemy;
    }
}

public class EnemyJobScheduler : MonoBehaviour
{
    private Dictionary<string, Job> jobMap = new Dictionary<string, Job>();
    private List<Job> unassignedJobsQueue = new List<Job>();
    private List<Job> assignedJobsQueue = new List<Job>();
    private Graph pathGraph;
    private SurfaceOperations SurfaceOperations;
    public Pathfinder Pathfinder { get; set; }
    private List<Enemy> busyMobs = new List<Enemy>();
    private List<Enemy> freeMobs = new List<Enemy>();
    public List<Enemy> AllMobs = new List<Enemy>();
    public List<Mob> Mobs = new List<Mob>();

    void Update()
    {
        while (assignedJobsQueue.Count != 0)
        {
            var job = assignedJobsQueue[0];
            assignedJobsQueue.Remove(job);
            MoveFreeMobToBusyMobs((Enemy)job.Mob);
            SetJobToEnemy(job);
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
    public void AddEnemy(Enemy enemy)
    {
        enemy.SurfaceOperations = SurfaceOperations;
        enemy.Pathfinder = Pathfinder;
        freeMobs.Add(enemy);
        AllMobs.Add(enemy);
    }

    public void AddWorker(Mob mob)
    {
        Mobs.Add(mob);
    }

    private bool SomeJobLeft()
    {
        return unassignedJobsQueue.Count != 0 && freeMobs.Count != 0;
    }

    private void AssignWork()
    {
        var freeMobsClone = new List<Enemy>(freeMobs);
        var job = unassignedJobsQueue[0];
        var parallelJob = ParallelComputations.CreateParallelDistancesJob(freeMobsClone.Select(mod => mod.Position).ToList(), job.Destination);
        JobHandle handle = parallelJob.Schedule(freeMobsClone.Count, 4);
        handle.Complete();

        PriorityQueue<JobEnemy, float> minDistances = new PriorityQueue<JobEnemy, float>(0);

        for (var i = 0; i < parallelJob.Result.Length; i++)
        {
            minDistances.Enqueue(new JobEnemy(job, freeMobsClone[i]), parallelJob.Result[i]);
        }
        ParallelComputations.FreeDistancesJobMemory(parallelJob);
        while (minDistances.Count != 0)
        {
            var minDistance = minDistances.Dequeue();
            var path = Pathfinder.FindPath(minDistance.Enemy.Position, minDistance.Job.Destination, true);
            if (path != null)
            {
                job.Mob = minDistance.Enemy;
                job.Mob.Job = job;
                job.Path = path; ;
                MoveUnassignedJobToAssignedJobs(job);
                return;
            }
        }
        unassignedJobsQueue.Remove(job);
        unassignedJobsQueue.Add(job);
    }

    private void SetJobToEnemy(Job job)
    {
        // if (job.Type == JobType.CARRYING) SetCarryingJob((CarrierJob)job);
        // else if (job.Type == JobType.FILL || job.Type == JobType.DIG) SetEnemyJob((JobEnemy)job);
    }

    // private void SetEnemyJob(EnemyJob job)
    // {
    //     var enemy = job.Enemy;
    //     var path = job.Path;

    //     job.Cancel = () =>
    //     {
    //         CancelJob(job);
    //     };

    //     enemy.SetRunAnimation();
    //     enemy.SetState(new GoToState(enemy));
    //     enemy.SetPath(path);
    // }

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
            MoveBusyMobToFreeMobs((Enemy)job.Mob);
            job.Mob.SetState(new PatrolState((Enemy)job.Mob));
            job.Mob.Path = null;
        }
    }

    public void Remove(Job job)
    {
        assignedJobsQueue.Remove(job);
        unassignedJobsQueue.Remove(job);
        jobMap.Remove(job.Id);
    }

    private void MoveFreeMobToBusyMobs(Enemy freeEnemy)
    {
        freeMobs.Remove(freeEnemy);
        busyMobs.Add(freeEnemy);
    }
    private void MoveBusyMobToFreeMobs(Enemy busyEnemy)
    {
        busyMobs.Remove(busyEnemy);
        freeMobs.Add(busyEnemy);
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
