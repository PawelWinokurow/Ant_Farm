using System.Collections.Generic;
using UnityEngine;


public class Store : MonoBehaviour
{
    public List<Mob> allMobs = new List<Mob>();
    public List<Mob> allEnemies = new List<Mob>();

    public void AddMob(Mob mob)
    {
        allMobs.Add(mob);
    }
    public void AddScorpion(Mob mob)
    {
        allEnemies.Add(mob);
    }
}

