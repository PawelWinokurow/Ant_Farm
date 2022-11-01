using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildWallsTest : MonoBehaviour
{
    private Surface surf;
    [Range(0, 100)]
    public int wallPercentage = 25;
    private int wallPercentageOld = 25;

    public void Init(Surface surf)
    {
        this.surf = surf;
        CreateWalls();
    }

    private void Update()
    {
        if (wallPercentage != wallPercentageOld)
        {
            CreateWalls();
            wallPercentageOld = wallPercentage;
        }
    }


    void CreateWalls()
    {
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        surf.PathGraph.ResetAllEdgesToWalkable();
        for (int i = 1; i < surf.allHex.Length - 1; i++)
        {
            if (UnityEngine.Random.Range(0, 100f) < wallPercentage)
            {
                surf.AddBlock(surf.allHex[i]);
            }
            else
            {
                surf.AddGround(surf.allHex[i]);
            }
        }
    }
}
