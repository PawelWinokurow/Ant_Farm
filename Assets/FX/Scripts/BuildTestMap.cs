using System;
using UnityEngine;


public class BuildTestMap : MonoBehaviour
{

    private Surface surface;
    public int wallProbability = 50;
    public int foodProbability = 1;

    public void Init(Surface surface)
    {
        this.surface = surface;
        CreateWalls();
    }

    public void CreateWalls()
    {
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        surface.pathGraph.ResetAllEdgesToWalkable();
        for (int i = 1; i < surface.hexagons.Length - 1; i++)
        {
            var probability = UnityEngine.Random.Range(0, 100f);
            if (probability <= wallProbability)
            {
                surface.AddBlock(surface.hexagons[i]);
                surface.pathGraph.ProhibitHexagon(surface.hexagons[i]);
            }
            else if (probability <= wallProbability + foodProbability)
            {
                surface.AddFood(surface.hexagons[i]);
            }
            else
            {
                surface.AddGround(surface.hexagons[i]);
            }
        }
        surface.AddBase();
    }

}
