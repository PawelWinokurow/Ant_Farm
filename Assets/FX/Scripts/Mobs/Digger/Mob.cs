using UnityEngine;
using UnityEngine.AI;

public interface Mob
{
    bool HasJob();
    void SetJob(Job job);
    void SetPath(Path path);
}
