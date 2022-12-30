using System.Collections.Generic;
using UnityEngine;


public class Store : MonoBehaviour
{
    public List<Targetable> allAllies = new List<Targetable>();
    public List<Targetable> allEnemies = new List<Targetable>();
    private static object monitorLock = new object();

    public void AddAlly(Targetable mob)
    {
        lock (monitorLock)
        {
            allAllies.Add(mob);
        }
    }
    public void DeleteAlly(Targetable mob)
    {
        lock (monitorLock)
        {
            allAllies.Remove(mob);
        }
    }
    public void AddEnemy(Targetable mob)
    {
        lock (monitorLock)
        {
            allEnemies.Add(mob);
        }
    }
    public void DeleteEnemy(Targetable mob)
    {
        lock (monitorLock)
        {
            allEnemies.Remove(mob);
        }
    }
}

