using System.Collections;
using UnityEngine;


public class BuryAssignment
{
    private IEnumerator coroutine;
    public void DoWork(Hexagon hex, Mob mob)
    {
        Debug.Log("Bury");
        mob.SetJob(null);
    }

}