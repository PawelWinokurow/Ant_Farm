using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int ACCESS_MASK_FLOOR = 1;
    public int ACCESS_MASK_FLOOR_SOIL = 2;
    public int ACCESS_MASK_FULL = 16;
}