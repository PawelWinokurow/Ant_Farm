using UnityEngine;
using UnityEngine.AI;

public interface Mob
{

    public bool HasJob();
    public void SetJob(Job job);
    public void SetPath(Path path);
}
