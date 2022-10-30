using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{

    public bool isWall;
    public bool isDig;
    public bool isGround;
    public bool isBuild;
    public bool isSpawn;
    public float hp = 1f;
    public int id;
    public int cost;

    public List<int> neighbors;

    public bool IsDigabble()
    {
        return isWall;
    }

    public bool IsBuildable()
    {
        return isGround;
    }
    public bool IsInSpawnArea()
    {
        return isSpawn;
    }
    public void SetDigable()
    {
        isDig = true; ;
    }
    public void SetBuildable()
    {
        isBuild = true; ;
    }

}

