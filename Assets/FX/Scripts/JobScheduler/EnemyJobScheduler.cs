using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;

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
    private Graph pathGraph;
    private SurfaceOperations SurfaceOperations;
    public Pathfinder Pathfinder { get; set; }
    private List<Enemy> busyEnemies = new List<Enemy>();
    private List<Enemy> freeEnemies = new List<Enemy>();
    public List<Enemy> AllMobs = new List<Enemy>();
    public List<Mob> Mobs = new List<Mob>();

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

    public void SetSurfaceOperations(SurfaceOperations setSurfaceOperations)
    {
        this.SurfaceOperations = setSurfaceOperations;
    }
    public void AddEnemy(Enemy enemy)
    {
        enemy.SurfaceOperations = SurfaceOperations;
        enemy.Pathfinder = Pathfinder;
        freeEnemies.Add(enemy);
        AllMobs.Add(enemy);
    }

    public void AddWorker(Mob mob)
    {
        Mobs.Add(mob);
    }

    private void AssignWork()
    {
        var freeEnemiesClone = new List<Enemy>(freeEnemies);
        KDTree mobPositionsTree = new KDTree(Mobs.Select(mob => mob.Position).ToArray());
        KDQuery query = new KDQuery();
        foreach (Enemy enemy in freeEnemiesClone)
        {
            List<int> queryResults = new List<int>();
            query.Radius(mobPositionsTree, enemy.Position, 20f, queryResults);
            if (queryResults.Count == 0) { continue; }
            for (int i = 0; i < queryResults.Count; i++)
            {
                var target = Mobs[queryResults[i]];
                var path = Pathfinder.FindPath(enemy.Position, target.Position, true);

                if (path != null)
                {
                    var job = new EnemyJob($"{enemy.Id}_{target.Id}", enemy, target);
                    job.Path = path;
                    SetJobToEnemy(job);
                    return;
                }

            }
        }
    }

    private void SetJobToEnemy(EnemyJob job)
    {
        var enemy = job.Enemy;
        MoveFreeMobToBusyMobs(enemy);
        job.Cancel = () =>
        {
            MoveBusyMobToFreeMobs(enemy);
        };
        enemy.Job = job;
        var path = job.Path;
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
