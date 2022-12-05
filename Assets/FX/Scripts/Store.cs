using System.Collections.Generic;
using UnityEngine;


public class Store : MonoBehaviour
{
    public List<Mob> allAllies = new List<Mob>();
    public List<Mob> allEnemies = new List<Mob>();
    private static object monitorLock = new object();

    public void AddAlly(Mob mob)
    {
        lock (monitorLock)
        {
            allAllies.Add(mob);
        }
    }
    public void DeleteAlly(Mob mob)
    {
        lock (monitorLock)
        {
            allAllies.Remove(mob);
        }
    }
    public void AddEnemy(Mob mob)
    {
        lock (monitorLock)
        {
            allEnemies.Add(mob);
        }
    }
    public void DeleteEnemy(Mob mob)
    {
        lock (monitorLock)
        {
            allEnemies.Remove(mob);
        }
    }
}

