using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int ACCESS_MASK_FLOOR = 1;
    public int ACCESS_MASK_ALL = 3;
    public enum ACCESS_MASK
    {
        FLOOR = 1, ALL = 3
    }
}