using System;
using UnityEngine;
public class EnemyJob : Job
{
    public string id { get; set; }
    public Enemy enemy { get; set; }
    public Mob target { get; set; }
    public JobType type { get; set; }
    public Path path { get; set; }
    Vector3 Job.destination { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    FloorHexagon Job.hex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    Mob Job.mob { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action Cancel { get; set; }

    public EnemyJob(string id, Enemy enemy, Mob target)
    {
        this.id = id;
        this.enemy = enemy;
        this.target = target;
        type = JobType.ATTACK;
    }
}