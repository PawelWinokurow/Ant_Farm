using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;
using WorkerNamespace;

public class WorkerJobScheduler : MonoBehaviour
{
    private Dictionary<string, WorkerJob> jobMap = new Dictionary<string, WorkerJob>();
    private List<WorkerJob> unassignedBuildingJobsQueue = new List<WorkerJob>();
    private List<WorkerJob> assignedBuildingJobsQueue = new List<WorkerJob>();
    private List<WorkerJob> unassignedCarryingJobsQueue = new List<WorkerJob>();
    private List<WorkerJob> assignedCarryingJobsQueue = new List<WorkerJob>();
    private Graph pathGraph;
    private SurfaceOperations surfaceOperations;
    public Pathfinder pathfinder { get; set; }
    private List<Worker> carryingWorkers = new List<Worker>();
    private List<Worker> buildingWorkers = new List<Worker>();
    private List<Worker> freeWorkers = new List<Worker>();
    public List<Worker> allWorkers = new List<Worker>();
    private static object monitorLock = new object();
    public GameSettings gameSettings;
    public List<FloorHexagon> foodHexagons = new List<FloorHexagon>();


    private bool someUnassignedBuildingJobLeft { get => unassignedBuildingJobsQueue.Count != 0; }
    private bool someAssignedBuildingJobLeft { get => assignedBuildingJobsQueue.Count != 0; }
    private bool someUnassignedCarryingJobLeft { get => unassignedCarryingJobsQueue.Count != 0; }
    private bool someAssignedCarryingJobLeft { get => assignedCarryingJobsQueue.Count != 0; }
    private bool someFreeWorkersLeft { get => freeWorkers.Count != 0; }
    private bool someCarryingWorkersLeft { get => carryingWorkers.Count != 0; }
    private bool someFoodLeft { get => foodHexagons.Count != 0; }

    void Start()
    {
        gameSettings = Settings.Instance.gameSettings;
    }

    void Update()
    {

        if (someFoodLeft && ((CollectingHexagon)(foodHexagons.First().child)).Quantity <= 0) foodHexagons.RemoveAt(0);
        if (!someFoodLeft) SetPathToFood();

        while (someAssignedBuildingJobLeft)
        {
            var job = assignedBuildingJobsQueue.First();
            assignedBuildingJobsQueue.Remove(job);
            MoveFreeMobToBusyMobs(job);
            SetJobToWorker(job);
        }
        while (someAssignedCarryingJobLeft)
        {
            var job = assignedCarryingJobsQueue.First();
            assignedCarryingJobsQueue.Remove(job);
            MoveFreeMobToBusyMobs(job);
            SetJobToWorker(job);
        }
    }

    private void SetPathToFood()
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

    public void StartJobScheuler()
    {
        StartCoroutine(DistributeJobsCoroutine());
    }

    IEnumerator DistributeJobsCoroutine()
    {
        while (true)
        {
            if (someUnassignedBuildingJobLeft)
            {
                if (someFreeWorkersLeft) AssignBuildingWork();
                else if (!someFreeWorkersLeft && someCarryingWorkersLeft && ReleaseCarryingWorker())
                {
                    AssignBuildingWork();
                }
            }
            else
            {
                if (someFreeWorkersLeft && someFoodLeft)
                {
                    AssignCarryingWork();
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private bool ReleaseCarryingWorker()
    {
        var nearestWorker = carryingWorkers.Where(worker => worker.carryingWeight == 0).OrderBy(worker => Distance.Manhattan(worker.position, foodHexagons[0].position)).FirstOrDefault();
        if (nearestWorker == null) return false;
        nearestWorker.CancelJob();
        return true;
    }

    private bool IsPathToFoodExists()
    {
        return pathfinder.FindPath(surfaceOperations.surface.baseHex.vertex.neighbours[0].position, foodHexagons[0].position, gameSettings.ACCESS_MASK_FLOOR, SearchType.NEAREST_CENTRAL_VERTEX) != null;
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
            carryingWorkers.Remove(worker);
            buildingWorkers.Remove(worker);
            freeWorkers.Remove(worker);
            worker.CancelJob();
        }
        worker.SetState(new DeadState(worker));
    }

    private void AssignBuildingWork()
    {

        var freeWorkersClone = new List<Worker>(freeWorkers);
        var job = unassignedBuildingJobsQueue.First();

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
                MoveUnassignedJobToAssignedJobs(job, assignedBuildingJobsQueue, unassignedBuildingJobsQueue);
                return;
            }
        }
        unassignedBuildingJobsQueue.Remove(job);
        unassignedBuildingJobsQueue.Add(job);
    }

    private void AssignCarryingWork()
    {
        var freeWorker = freeWorkers.First();
        var foodHex = foodHexagons.First();
        var job = new CarrierJob(foodHex, (BaseHexagon)surfaceOperations.surface.baseHex.child);
        var path = pathfinder.FindPath(freeWorker.position, job.destination, freeWorker.accessMask, SearchType.NEAREST_CENTRAL_VERTEX);
        if (path != null)
        {
            job.worker = freeWorker;
            job.worker.job = job;
            job.path = path;
            MoveUnassignedJobToAssignedJobs(job, assignedCarryingJobsQueue, unassignedCarryingJobsQueue);
            return;
        }
    }

    private FloorHexagon FindNearestFood()
    {
        var baseNeighbour = surfaceOperations.surface.baseHex.vertex.neighbours.First();
        var foodHexagons = surfaceOperations.surface.hexagons.Where(hex => hex.type == HexType.FOOD).ToList();

        if (foodHexagons.Count != 0)
        {
            KDTree workerPositionsTree = new KDTree(foodHexagons.Select(hex => hex.position).ToArray());
            KDQuery query = new KDQuery();
            List<int> queryResults = new List<int>();
            query.KNearest(workerPositionsTree, baseNeighbour.position, 1, queryResults);
            return foodHexagons[queryResults.First()];
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
            return unassignedBuildingJobsQueue.Any(item => item.id == jobId);
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
            return assignedBuildingJobsQueue.Any(item => item.id == jobId);
        }
    }

    public void AssignJob(WorkerJob job)
    {
        //TODO
        if (!IsJobAlreadyCreated(job) && !IsJobUnassigned(job))
        {
            jobMap.Add(job.id, job);
            unassignedBuildingJobsQueue.Add(job);
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
            if (job.worker != null)
            {
                if (job.type == JobType.MOUNT || job.type == JobType.DEMOUNT)
                {
                    assignedBuildingJobsQueue.Remove(job);
                    unassignedBuildingJobsQueue.Remove(job);
                    MoveBusyMobToFreeMobs(job.worker, buildingWorkers);
                }
                else if (job.type == JobType.CARRYING)
                {

                    assignedCarryingJobsQueue.Remove(job);
                    unassignedCarryingJobsQueue.Remove(job);
                    MoveBusyMobToFreeMobs(job.worker, carryingWorkers);
                }
                job.worker.SetState(new IdleState(job.worker));
                job.worker.path = null;
            }
        }
    }

    private void MoveFreeMobToBusyMobs(WorkerJob job)
    {
        if (job.type == JobType.MOUNT || job.type == JobType.DEMOUNT)
        {
            MoveFreeMobToBusyMobs(job.worker, buildingWorkers);
        }
        else if (job.type == JobType.CARRYING)
        {
            MoveFreeMobToBusyMobs(job.worker, carryingWorkers);
        }
    }

    private void MoveFreeMobToBusyMobs(Worker freeWorker, List<Worker> busyWorkers)
    {
        lock (monitorLock)
        {
            freeWorkers.Remove(freeWorker);
            busyWorkers.Add(freeWorker);
        }
    }
    private void MoveBusyMobToFreeMobs(Worker busyWorker, List<Worker> busyWorkers)
    {
        lock (monitorLock)
        {
            busyWorkers.Remove(busyWorker);
            freeWorkers.Add(busyWorker);
        }
    }

    private void MoveUnassignedJobToAssignedJobs(WorkerJob unassignedJob, List<WorkerJob> assignedJobQueue, List<WorkerJob> unassignedJobQueue)
    {
        lock (monitorLock)
        {
            unassignedJobQueue.Remove(unassignedJob);
            assignedJobQueue.Add(unassignedJob);
        }
    }
    private void MoveAssignedJobToUnssignedJobs(WorkerJob assignedJob, List<WorkerJob> assignedJobQueue, List<WorkerJob> unassignedJobQueue)
    {
        lock (monitorLock)
        {
            assignedJobQueue.Remove(assignedJob);
            unassignedJobQueue.Add(assignedJob);
        }
    }

}
