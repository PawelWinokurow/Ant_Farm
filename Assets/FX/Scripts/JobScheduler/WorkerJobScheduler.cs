using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;
using WorkerNamespace;

public class WorkerJobScheduler : MonoBehaviour
{
    private Dictionary<string, WorkerJob> jobMap = new Dictionary<string, WorkerJob>();
    private List<WorkerJob> unassignedJobsQueue = new List<WorkerJob>();
    private List<WorkerJob> assignedJobsQueue = new List<WorkerJob>();
    private Graph pathGraph;
    private SurfaceOperations surfaceOperations;
    public Pathfinder pathfinder { get; set; }
    private List<Worker> busyWorkers = new List<Worker>();
    private List<Worker> freeWorkers = new List<Worker>();
    public List<Worker> allWorkers = new List<Worker>();
    private static object monitorLock = new object();
    public GameSettings gameSettings;
    public List<Hexagon> foodHexagons = new List<Hexagon>();

    void Start()
    {
        gameSettings = Settings.Instance.gameSettings;
    }

    void Update()
    {
        PathToFood();
        while (assignedJobsQueue.Count != 0)
        {
            var job = assignedJobsQueue[0];
            assignedJobsQueue.Remove(job);
            MoveFreeMobToBusyMobs(job.worker);
            SetJobToWorker(job);
        }
    }

    private void PathToFood()
    {
        if (foodHexagons.Count == 0)
        {
            var nearestFoodHex = FindNearestFood();
            if (nearestFoodHex != null)
            {
                var path = pathfinder.FindPath(surfaceOperations.surface.baseHex.vertex.neighbours[0].position, nearestFoodHex.position, gameSettings.ACCESS_MASK_FLOOR + gameSettings.ACCESS_MASK_SOIL + gameSettings.ACCESS_MASK_BASE, SearchType.NEAREST_CENTRAL_VERTEX);
                if (path != null)
                {
                    var soilHexagons = path.wayPoints
                        .Select(edge => edge.floorHexagon)
                        .Distinct()
                        .Where(hex => hex.type == HexType.SOIL)
                        .ToList();
                    soilHexagons.ForEach(hex => { surfaceOperations.surface.PlaceIcon(hex, SliderValue.DEMOUNT); AssignJob(new BuildJob(hex, hex.transform.position, JobType.DEMOUNT)); });
                    foodHexagons.Add(nearestFoodHex);
                }
            }
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
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetSurfaceOperations(SurfaceOperations setSurfaceOperations)
    {
        this.surfaceOperations = setSurfaceOperations;
    }
    public void AddWorker(Worker worker)
    {

        worker.surfaceOperations = surfaceOperations;
        worker.pathfinder = pathfinder;
        worker.Kill = () => Kill(worker);
        lock (monitorLock)
        {
            freeWorkers.Add(worker);
            allWorkers.Add(worker);
        }
    }

    private void Kill(Worker worker)
    {
        lock (monitorLock)
        {
            busyWorkers.Remove(worker);
            freeWorkers.Remove(worker);
            worker.CancelJob();
            // Destroy(worker);
        }
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

        KDTree workerPositionsTree = new KDTree(freeWorkersClone.Select(worker => worker.position).ToArray());
        KDQuery query = new KDQuery();
        List<int> queryResults = new List<int>();
        query.KNearest(workerPositionsTree, job.destination, freeWorkersClone.Count, queryResults);
        queryResults.Reverse();
        foreach (int i in queryResults)
        {
            var path = pathfinder.FindPath(freeWorkersClone[i].position, job.destination, freeWorkersClone[i].accessMask, SearchType.NEAREST_CENTRAL_VERTEX);
            if (path != null)
            {
                job.worker = freeWorkersClone[i];
                job.worker.job = job;
                job.path = path; ;
                MoveUnassignedJobToAssignedJobs(job);
                return;
            }
        }
        unassignedJobsQueue.Remove(job);
        unassignedJobsQueue.Add(job);
    }

    private FloorHexagon FindNearestFood()
    {
        var baseNeighbour = surfaceOperations.surface.baseHex.vertex.neighbours[0];
        var foodHexagons = surfaceOperations.surface.hexagons.Where(hex => hex.type == HexType.FOOD).ToList();

        if (foodHexagons.Count != 0)
        {
            KDTree workerPositionsTree = new KDTree(foodHexagons.Select(hex => hex.position).ToArray());
            KDQuery query = new KDQuery();
            List<int> queryResults = new List<int>();
            query.KNearest(workerPositionsTree, baseNeighbour.position, 1, queryResults);
            return foodHexagons[queryResults[0]];
        }
        return null;
    }

    private void SetJobToWorker(WorkerJob job)
    {
        if (job.type == JobType.CARRYING) SetCarryingJob((CarrierJob)job);
        else if (job.type == JobType.MOUNT || job.type == JobType.DEMOUNT) SetBuildJob((BuildJob)job);
    }

    private void SetCarryingJob(CarrierJob job)
    {
        var worker = job.worker;
        var path = job.path;

        job.storageHexagon = surfaceOperations.NearestBaseHexagon();
        job.collectingHexagon = (CollectingHexagon)(job.hex.child);
        job.collectingHexagon.AssignWorker(worker);

        job.Cancel = () =>
        {
            CancelJob(job);
        };

        worker.SetPath(path);
        worker.SetRunAnimation();
        worker.SetState(new GoToState(worker));
    }
    private void SetBuildJob(BuildJob job)
    {
        var worker = job.worker;
        var path = job.path;

        job.Cancel = () =>
        {
            CancelJob(job);
        };

        worker.SetRunAnimation();
        worker.SetState(new GoToState(worker));
        worker.SetPath(path);
    }

    public bool IsJobAlreadyCreated(WorkerJob job)
    {
        lock (monitorLock)
        {
            return IsJobAlreadyCreated(job.id);
        }
    }
    public bool IsJobAlreadyCreated(string jobId)
    {
        return jobMap.ContainsKey(jobId);
    }
    public bool IsJobUnassigned(WorkerJob job)
    {
        return IsJobUnassigned(job.id);
    }
    public bool IsJobUnassigned(string jobId)
    {
        lock (monitorLock)
        {
            return unassignedJobsQueue.Any(item => item.id == jobId);
        }
    }
    public bool IsJobAssigned(WorkerJob job)
    {
        return IsJobAssigned(job.id);
    }
    public bool IsJobAssigned(string jobId)
    {
        lock (monitorLock)
        {
            return assignedJobsQueue.Any(item => item.id == jobId);
        }
    }

    public void AssignJob(WorkerJob job)
    {
        if (!IsJobAlreadyCreated(job) && !IsJobUnassigned(job))
        {
            jobMap.Add(job.id, job);
            unassignedJobsQueue.Add(job);
        }
    }

    public void CancelJob(string jobId)
    {
        if (jobMap.ContainsKey(jobId))
        {
            CancelJob(jobMap[jobId]);
        }
    }

    public void CancelJob(WorkerJob job)
    {
        lock (monitorLock)
        {
            Remove(job);
            if (job.worker != null)
            {
                MoveBusyMobToFreeMobs(job.worker);
                job.worker.SetState(new IdleState(job.worker));
                job.worker.path = null;
            }
        }
    }


    private void Remove(WorkerJob job)
    {
        lock (monitorLock)
        {
            assignedJobsQueue.Remove(job);
            unassignedJobsQueue.Remove(job);
            jobMap.Remove(job.id);
        }
    }

    private void MoveFreeMobToBusyMobs(Worker freeWorker)
    {
        lock (monitorLock)
        {
            freeWorkers.Remove(freeWorker);
            busyWorkers.Add(freeWorker);
        }
    }
    private void MoveBusyMobToFreeMobs(Worker busyWorker)
    {
        lock (monitorLock)
        {
            busyWorkers.Remove(busyWorker);
            freeWorkers.Add(busyWorker);
        }
    }

    private void MoveUnassignedJobToAssignedJobs(WorkerJob unassignedJob)
    {
        lock (monitorLock)
        {
            unassignedJobsQueue.Remove(unassignedJob);
            assignedJobsQueue.Add(unassignedJob);
        }
    }
    private void MoveAssignedJobToUnssignedJobs(WorkerJob assignedJob)
    {
        lock (monitorLock)
        {
            assignedJobsQueue.Remove(assignedJob);
            unassignedJobsQueue.Add(assignedJob);
        }
    }

}
