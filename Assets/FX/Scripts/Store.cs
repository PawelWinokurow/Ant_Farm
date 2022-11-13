using System.Collections.Generic;
using UnityEngine;


public class Store : MonoBehaviour
{
    public List<Mob> allMobs = new List<Mob>();

    public void AddMob(Mob mob)
    {
        allMobs.Add(mob);
    }
}

