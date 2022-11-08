using System;
using UnityEngine;


public class BuildTestMap : MonoBehaviour
{

    private Surface Surface;
    public int wallProbability = 50;
    public int foodProbability = 1;

    public void Init(Surface Surface)
    {
        this.Surface = Surface;
        CreateWalls();
    }

    public void CreateWalls()
    {
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        Surface.PathGraph.ResetAllEdgesToWalkable();
        for (int i = 1; i < Surface.Hexagons.Length - 1; i++)
        {
            var probability = UnityEngine.Random.Range(0, 100f);
            if (probability <= wallProbability)
            {
                Surface.AddBlock(Surface.Hexagons[i]);
                Surface.PathGraph.ProhibitHexagon(Surface.Hexagons[i]);
            }
            else if (probability <= wallProbability + foodProbability)
            {
                Surface.AddFood(Surface.Hexagons[i]);
            }
            else
            {
                Surface.AddGround(Surface.Hexagons[i]);
            }
        }
        Surface.AddBase();
    }

}
