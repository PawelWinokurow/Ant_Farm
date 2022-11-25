using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public readonly int ACCESS_MASK_FLOOR = 1;
    public readonly int ACCESS_MASK_FLOOR_SOIL = 2;
    public readonly int SCORPION_HP = 250;
    public readonly int SOLDIER_HP = 150;
    public readonly int WORKER_HP = 100;
}