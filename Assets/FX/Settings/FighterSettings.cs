using UnityEngine;

[CreateAssetMenu]
public class FighterSettings : ScriptableObject
{
    public int HP;
    public int ATTACK_STRENGTH;
    public int PATROL_MOVEMENT_SPEED;
    public int PATROL_ACCESS_MASK;
    public int FOLLOWING_MOVEMENT_SPEED;
    public int FOLLOWING_ACCESS_MASK;
}