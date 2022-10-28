using UnityEngine;
using System.Collections.Generic;

public interface Mob
{
    public Vector3 CurrentPosition { get; set; }
    public List<Vector3> WayPoints { get; set; }
    public bool HasJob();
    public void SetJob(Job job);
    public void SetPath(Path path);
}
