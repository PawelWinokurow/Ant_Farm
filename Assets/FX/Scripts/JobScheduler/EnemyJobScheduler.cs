using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;


public class EnemyJobScheduler : MonoBehaviour
{
    private Graph pathGraph;
    private SurfaceOperations surfaceOperations;
    public Pathfinder pathfinder { get; set; }
    private List<Enemy> busyEnemies = new List<Enemy>();
    private List<Enemy> freeEnemies = new List<Enemy>();
    public List<Enemy> allWorkers = new List<Enemy>();
    public List<Mob> mobs = new List<Mob>();

    public void StartJobScheuler()
    {
        StartCoroutine(DistributeJobsCoroutine());
    }
    IEnumerator DistributeJobsCoroutine()
    {
        while (true)
        {
            if (freeEnemies.Count != 0)
            {
                AssignWork();
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void SetSurfaceOperations(SurfaceOperations surfaceOperations)
    {
        this.surfaceOperations = surfaceOperations;
    }
    public void AddEnemy(Enemy enemy)
    {
        enemy.surfaceOperations = surfaceOperations;
        enemy.pathfinder = pathfinder;
        freeEnemies.Add(enemy);
        allWorkers.Add(enemy);
    }

    public void AddWorker(Mob mob)
    {
        mobs.Add(mob);
    }

    private void AssignWork()
    {
        var freeEnemiesClone = new List<Enemy>(freeEnemies);
        KDTree mobPositionsTree = new KDTree(mobs.Select(mob => mob.position).ToArray());
        KDQuery query = new KDQuery();
        foreach (Enemy enemy in freeEnemiesClone)
        {
            List<int> queryResults = new List<int>();
            query.Radius(mobPositionsTree, enemy.position, 120f, queryResults);
            if (queryResults.Count == 0) { continue; }
            for (int i = 0; i < queryResults.Count; i++)
            {
                var target = mobs[queryResults[i]];
                var path = pathfinder.FindPath(enemy.position, target.position, true);
                if (path != null)
                {
                    var job = new EnemyJob($"{enemy.id}_{target.id}", enemy, target);
                    job.path = path;
                    SetJobToEnemy(job);
                    return;
                }

            }
        }
    }

    private void SetJobToEnemy(EnemyJob job)
    {
        var enemy = job.enemy;
        MoveFreeMobToBusyMobs(enemy);
        job.Cancel = () =>
        {
            MoveBusyMobToFreeMobs(enemy);
        };
        enemy.job = job;
        var path = job.path;
        Debug.Log("in");
        enemy.SetState(new FollowingState(enemy));
        enemy.SetPath(path);
    }

    private void MoveFreeMobToBusyMobs(Enemy freeEnemy)
    {
        freeEnemies.Remove(freeEnemy);
        busyEnemies.Add(freeEnemy);
    }
    private void MoveBusyMobToFreeMobs(Enemy busyEnemy)
    {
        busyEnemies.Remove(busyEnemy);
        freeEnemies.Add(busyEnemy);
    }

}
