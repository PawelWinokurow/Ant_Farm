using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public readonly int ACCESS_MASK_FLOOR = 1;
    public readonly int ACCESS_MASK_SPIKES = 1;
    public readonly int ACCESS_MASK_SOIL = 2;
    public readonly int ACCESS_MASK_BASE = 4;
    public readonly int ACCESS_MASK_STONE = 8;
    public readonly int ACCESS_MASK_PROHIBIT = 8;
    public readonly int PATHFINDER_MAX_STEPS = 1000;
    public readonly int EDGE_WEIGHT_NORMAL = 1;
    public readonly int EDGE_WEIGHT_OBSTACLE = 10;
}