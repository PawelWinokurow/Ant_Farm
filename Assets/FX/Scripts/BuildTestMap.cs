using System;
using UnityEngine;


public class BuildTestMap : MonoBehaviour
{
    private Surface surface;
    public int wallProbability = 80;
    public int foodProbability = 1;
    private GameSettings gameSettings;

    public void Init()
    {
        gameSettings = Settings.Instance.gameSettings;
        surface = Surface.Instance;
        CreateWalls();
    }

    public void CreateWalls()
    {
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
        surface.pathGraph.ResetAllEdgesToWalkable();
        for (int i = 1; i < surface.hexagons.Length - 1; i++)
        {
            surface.ClearHex(surface.hexagons[i]);
            var probability = UnityEngine.Random.Range(0, 100f);
            if (probability <= wallProbability)
            {
                surface.AddSoil(surface.hexagons[i]);
                surface.pathGraph.SetAccesabillity(surface.hexagons[i], gameSettings.ACCESS_MASK_SOIL, gameSettings.EDGE_WEIGHT_OBSTACLE);
            }
            else if (probability <= wallProbability + foodProbability)
            {
                surface.AddFood(surface.hexagons[i]);
                surface.pathGraph.SetAccesabillity(surface.hexagons[i], gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
            }
            else
            {
                surface.AddGround(surface.hexagons[i]);
                surface.pathGraph.SetAccesabillity(surface.hexagons[i], gameSettings.ACCESS_MASK_FLOOR, gameSettings.EDGE_WEIGHT_NORMAL);
            }
        }
        surface.AddBase();
    }

}
