using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int ACCESS_MASK_FLOOR = 1;
    public int ACCESS_MASK_SPIKES = 1;
    public int ACCESS_MASK_SOIL = 2;
    public int ACCESS_MASK_BASE = 4;
    public int ACCESS_MASK_STONE = 8;
    public int ACCESS_MASK_PROHIBIT = 8;
    public int PATHFINDER_MAX_STEPS = 1000;
    public int EDGE_WEIGHT_NORMAL = 1;
    public int EDGE_WEIGHT_OBSTACLE = 10;
}