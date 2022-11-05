using System;
using UnityEngine;


public class BuildWallsTest : MonoBehaviour
{

    private Surface Surface;
    [Range(0, 100)]
    public int wallPercentage = 25;
    private int wallPercentageOld = 25;

    public void Init(Surface Surface)
    {
        this.Surface = Surface;
        CreateWalls();
    }

    private void Update()
    {
        // if (wallPercentage != wallPercentageOld)
        // {
        //     CreateWalls();
        //     wallPercentageOld = wallPercentage;
        // }
    }


    public void CreateWalls()
    {
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        Surface.PathGraph.ResetAllEdgesToWalkable();
        for (int i = 1; i < Surface.Hexagons.Length - 1; i++)
        {
            if (UnityEngine.Random.Range(0, 100f) < wallPercentage)
            {
                Surface.AddBlock(Surface.Hexagons[i]);
                Surface.PathGraph.ProhibitHexagon(Surface.Hexagons[i].Position);
            }
            else
            {
                Surface.AddGround(Surface.Hexagons[i]);
            }
        }
        Surface.AddBase();
        Surface.AddFood();
    }

}
