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
            
            

            Vector3 pos = surface.hexagons[i].position;
            Vector3 sidePos = SidePoint(pos);
            float spread = Vector3.Distance(surface.center, pos) / Vector3.Distance(surface.center, sidePos);

            float wallProb = wallProbability * Mathf.Pow(spread, 1/3f);
            float foodProb = foodProbability* spread;

            float rand = UnityEngine.Random.Range(0, 100f);
            
            if (rand <= wallProb)
            {
                surface.AddSoil(surface.hexagons[i]);
                surface.pathGraph.SetAccesabillity(surface.hexagons[i], gameSettings.ACCESS_MASK_SOIL, gameSettings.EDGE_WEIGHT_OBSTACLE);
            }
            else if (rand <= wallProb + foodProb)
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

    private Vector3 SidePoint(Vector3 pos)
    {
        Vector2 center = new Vector2(surface.center.x, surface.center.z);
        Vector2 point = new Vector2(pos.x, pos.z);
        float ax = (point.x - center.x);
        float ay = (point.y - center.y);
        Vector3 corner = surface.hexagons[surface.hexagons.Length - 1].position;
        Vector2[] points = new Vector2[4];
        points[0] = new Vector2(0, (0 - center.x) * ay / ax + center.y);
        points[1] = new Vector2(corner.x, (corner.x - center.x) * ay / ax + center.y);
        points[2] = new Vector2((0 - center.y) * ax / ay + center.x, 0);
        points[3] = new Vector2((corner.z - center.y) * ax / ay + center.x, corner.z);

        float distMin = Vector2.Distance(points[0], point);
        int nMin = 0;
        for (int i = 1; i < 4; i++)
        {
            float dist = Vector2.Distance(points[i], point);
            if (dist < distMin)
            {
                distMin = dist;
                nMin = i;
            }
        }
        return new Vector3(points[nMin].x, 0f, points[nMin].y);
    }

}
