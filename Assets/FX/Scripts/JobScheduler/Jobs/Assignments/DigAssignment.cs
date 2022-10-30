using System.Collections;
using UnityEngine;

public class DigAssignment
{
    private IEnumerator coroutine;
    public void DoWork(Hexagon hex, Mob mob)
    {
        Debug.Log("Dig");
        mob.SetJob(null);
    }
}