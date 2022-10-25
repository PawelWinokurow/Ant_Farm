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

    public bool IsDigabble()
    {
        return isWall && !isGround && !isDig && !isBuild;
    }

    public bool IsBuildable()
    {
        return isGround && !isWall && !isDig && !isBuild;
    }
    public bool IsInSpawnArea()
    {
        return isSpawn;
    }
    public void AssignDig()
    {
        isDig = true; ;
    }
    public void AssignBuild()
    {
        isBuild = true; ;
    }
}

