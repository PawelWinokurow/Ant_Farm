using UnityEngine;

[CreateAssetMenu]
public class SoldierSettings : ScriptableObject
{
    public readonly int HP = 150;
    public readonly int ATTACK_STRENGTH = 5;
    public readonly int FOLLOWING_MOVEMENT_SPEED = 10;
    public readonly int PATROL_MOVEMENT_SPEED = 5;
}