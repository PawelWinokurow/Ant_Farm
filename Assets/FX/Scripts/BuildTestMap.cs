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
                surface.pathGraph.SetAccesabillity(surface.hexagons[i], Settings.Instance.gameSettings.ACCESS_MASK_SOIL);
            }
            else if (probability <= wallProbability + foodProbability)
            {
                surface.AddFood(surface.hexagons[i]);
                surface.pathGraph.SetAccesabillity(surface.hexagons[i], Settings.Instance.gameSettings.ACCESS_MASK_FLOOR);
            }
            else
            {
                surface.AddGround(surface.hexagons[i]);
                surface.pathGraph.SetAccesabillity(surface.hexagons[i], Settings.Instance.gameSettings.ACCESS_MASK_FLOOR);
            }
        }
        surface.AddBase();
    }

}
