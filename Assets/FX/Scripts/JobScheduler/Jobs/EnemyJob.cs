using System;
using UnityEngine;
public class EnemyJob : Job
{
    public string Id { get; set; }
    public Enemy Enemy { get; set; }
    public Mob Target { get; set; }
    public JobType Type { get; set; }
    public Path Path { get; set; }
    Vector3 Job.Destination { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    FloorHexagon Job.Hex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    Mob Job.Mob { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action Cancel { get; set; }

    public EnemyJob(string id, Enemy enemy, Mob target)
    {
        Id = id;
        Enemy = enemy;
        Target = target;
        Type = JobType.ATTACK;
    }
}