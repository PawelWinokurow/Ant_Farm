using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public readonly int ACCESS_MASK_FLOOR = 1;
    public readonly int ACCESS_MASK_SOIL = 2;
    public readonly int ACCESS_MASK_BASE = 4;
    public readonly int ACCESS_MASK_PROHIBIT = 8;
}