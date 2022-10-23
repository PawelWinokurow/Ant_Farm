using UnityEngine;
using UnityEngine.AI;

public interface IAnt
{
    public Transform Transform { get; set; }
    public NavMeshAgent Agent { get; set; }
    bool HasJob();
    void SetJob(Job job);
    void SetPath(NavMeshPath path);
}
